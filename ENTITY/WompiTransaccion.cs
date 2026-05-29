using System.Collections.Generic;

namespace ENTITY
{
    public class WompiLinkResponse
    {
        public WompiLinkData Data { get; set; }
    }

    public class WompiLinkData
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public long AmountInCents { get; set; }
        public string Reference { get; set; }
        public List<WompiTransaction> Transactions { get; set; }
    }

    public class WompiTransaction
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public long AmountInCents { get; set; }
        public string Reference { get; set; }
        public string PaymentMethodType { get; set; }
        public WompiPaymentMethod PaymentMethod { get; set; }
    }

    public class WompiPaymentMethod
    {
        public string Type { get; set; }
        public WompiExtra Extra { get; set; }
    }

    public class WompiExtra
    {
        public string Bank { get; set; }
        public string PseBank { get; set; }
    }

    public class WompiTransactionResponse
    {
        public WompiTransactionData Data { get; set; }
    }

    public class WompiTransactionListResponse
    {
        public List<WompiTransactionData> Data { get; set; }
    }

    public class WompiTransactionData
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public long AmountInCents { get; set; }
        public string Reference { get; set; }
        public WompiPaymentMethod PaymentMethod { get; set; }
    }

    public class WompiTransferResponse
    {
        public WompiTransferData Data { get; set; }
    }

    public class WompiTransferData
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public long AmountInCents { get; set; }
        public string Reference { get; set; }
    }

    public class WompiErrorResponse
    {
        public List<WompiError> Errors { get; set; }
    }

    public class WompiError
    {
        public string Type { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
    }

    public class WompiFinancialInstitutionsResponse
    {
        public List<WompiFinancialInstitution> Data { get; set; } = new List<WompiFinancialInstitution>();
    }

    public class WompiFinancialInstitution
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
