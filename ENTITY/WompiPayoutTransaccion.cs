using System.Collections.Generic;

namespace ENTITY
{
    // ==================== REQUEST ====================
    
    public class WompiPayoutRequest
    {
        public string Reference { get; set; }
        public string AccountId { get; set; }
        public string PaymentType { get; set; }      // "OTHERS" para retiros de casino
        public string TransactionStatus { get; set; } // Solo en sandbox: "APPROVED" o "FAILED"
        public List<WompiPayoutTransaction> Transactions { get; set; }
    }

    public class WompiPayoutTransaction
    {
        public string LegalIdType { get; set; }   // "CC", "NIT", "CE"
        public string LegalId { get; set; }
        public string BankId { get; set; }         // UUID del banco en sistema payouts
        public string AccountType { get; set; }    // "AHORROS" o "CORRIENTE"
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Amount { get; set; }           // en centavos (monto * 100)
        public string Reference { get; set; }      // referencia única de esta transacción
    }

    // ==================== RESPONSE: POST /payouts ====================
    
    public class WompiPayoutResponse
    {
        public WompiPayoutData Data { get; set; }
    }

    public class WompiPayoutData
    {
        public string Id { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public long AmountInCents { get; set; }
        public int TotalTransactions { get; set; }
        public string DispersionDatetime { get; set; }
    }

    // ==================== RESPONSE: GET /payouts/{id} ====================
    
    public class WompiPayoutDetailResponse
    {
        public WompiPayoutDetailData Data { get; set; }
    }

    public class WompiPayoutDetailData
    {
        public string Id { get; set; }
        public string Status { get; set; }  // TOTAL_PAYMENT, PARTIAL_PAYMENT, FAILED, PROCESSING
        public List<WompiPayoutTransactionDetail> Transactions { get; set; }
    }

    public class WompiPayoutTransactionDetail
    {
        public string Id { get; set; }
        public string Status { get; set; }  // APPROVED, FAILED
        public string Reference { get; set; }
        public long AmountInCents { get; set; }
        public WompiPayoutFailureReason FailureReason { get; set; }
    }

    public class WompiPayoutFailureReason
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    // ==================== RESPONSE: GET /payouts/banks ====================
    
    public class WompiPayoutBanksResponse
    {
        public List<WompiPayoutBank> Data { get; set; }
    }

    public class WompiPayoutBank
    {
        public string Id { get; set; }      // UUID usado en payouts
        public string Name { get; set; }
        public string Code { get; set; }    // código bancario (puede mapear con BancoId existente)
        public bool IsActive { get; set; }
    }
}
