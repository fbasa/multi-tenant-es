
namespace UniEnroll.Infrastructure.EF.Sql;

public static class PaymentSql
{
    public const string Capture = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- ensure invoice exists
IF NOT EXISTS (SELECT 1 FROM Invoices WITH (READCOMMITTEDLOCK) WHERE Id=@invoice) 
BEGIN SELECT 'NotFound' AS Outcome, NULL AS PaymentId; ROLLBACK; RETURN; END;

-- idempotency via gateway txn id or key
IF @gatewayTxnId IS NOT NULL AND EXISTS(SELECT 1 FROM Payments WITH (READCOMMITTEDLOCK) WHERE GatewayTxnId=@gatewayTxnId)
BEGIN SELECT 'AlreadyCaptured' AS Outcome, (SELECT TOP(1) CAST(Id AS nvarchar(64)) FROM Payments WHERE GatewayTxnId=@gatewayTxnId); COMMIT; RETURN; END;

IF @idemKey IS NOT NULL AND EXISTS(SELECT 1 FROM Payments WITH (READCOMMITTEDLOCK) WHERE IdempotencyKey=@idemKey)
BEGIN SELECT 'AlreadyCaptured' AS Outcome, (SELECT TOP(1) CAST(Id AS nvarchar(64)) FROM Payments WHERE IdempotencyKey=@idemKey); COMMIT; RETURN; END;

DECLARE @id uniqueidentifier = NEWID();
INSERT INTO Payments (Id, InvoiceId, Amount, Currency, Method, Status, GatewayTxnId, IdempotencyKey, CreatedAt)
VALUES (@id, @invoice, @amount, @currency, @method, 'Succeeded', @gatewayTxnId, @idemKey, SYSUTCDATETIME());

UPDATE Invoices SET AmountPaid = ISNULL(AmountPaid,0) + @amount WHERE Id=@invoice;

SELECT 'Captured' AS Outcome, CAST(@id AS nvarchar(64)) AS PaymentId;
COMMIT;";

    public const string Refund = @"
SET XACT_ABORT ON;
BEGIN TRAN;

IF NOT EXISTS (SELECT 1 FROM Payments WITH (READCOMMITTEDLOCK) WHERE Id=@payment AND Status='Succeeded')
BEGIN SELECT 'NotFound' AS Outcome, NULL AS RefundId; ROLLBACK; RETURN; END;

DECLARE @invoice uniqueidentifier = (SELECT InvoiceId FROM Payments WHERE Id=@payment);

DECLARE @id uniqueidentifier = NEWID();
INSERT INTO Payments (Id, InvoiceId, ParentPaymentId, Amount, Currency, Method, Status, CreatedAt)
SELECT @id, p.InvoiceId, p.Id, @amount * -1, p.Currency, p.Method, 'Refunded', SYSUTCDATETIME()
FROM Payments p WHERE p.Id=@payment;

UPDATE Invoices SET AmountPaid = ISNULL(AmountPaid,0) - @amount WHERE Id=@invoice;

SELECT 'Refunded' AS Outcome, CAST(@id AS nvarchar(64)) AS RefundId;
COMMIT;";

    public const string GetStatus = @"
SELECT TOP(1) CAST(p.Id AS nvarchar(64)) AS PaymentId, p.Status, p.Amount, p.Currency
FROM Payments p WITH (READCOMMITTEDLOCK)
WHERE p.Id=@payment;";
}
