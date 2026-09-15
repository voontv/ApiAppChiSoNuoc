using System;
using System.Collections.Generic;

namespace ReadMeter.Api.OracleModels;

public partial class LogBankFix
{
    public string? DonVi { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public string? Err { get; set; }

    public string? InfocustCheck { get; set; }

    public string? InfocustCheckDetail { get; set; }

    public string? DebtCheck { get; set; }

    public string? DebtCheckDetail { get; set; }

    public string? DebtPayment { get; set; }

    public string? DebtPaymentDetail { get; set; }

    public string? DebtCancel { get; set; }

    public string? DebtCancelDetail { get; set; }

    public string? SystemCheck { get; set; }

    public string? SystemCheckDetail { get; set; }

    public decimal? InfocustCheckTout { get; set; }

    public decimal? DebtCheckTout { get; set; }

    public decimal? DebtPaymentTout { get; set; }

    public decimal? DebtCancelTout { get; set; }

    public decimal? SystemCheckTout { get; set; }

    public decimal? ToutItem4 { get; set; }

    public string? Template { get; set; }

    public string? DebtCheckV2 { get; set; }

    public string? DebtCheckV2Detail { get; set; }

    public string? DebtPaymentV2 { get; set; }

    public string? DebtPaymentV2Detail { get; set; }

    public decimal? DebtCheckV2Tout { get; set; }

    public decimal? DebtPaymentV2Tout { get; set; }
}
