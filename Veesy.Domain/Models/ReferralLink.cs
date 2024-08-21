using System;
using System.Collections.Generic;
using Veesy.Domain.Constants;

namespace Veesy.Domain.Models;

public class ReferralLink
{
    public Guid Id { get; set; }
    public string Endpoint { get; set; }
    public string RedirectUrl { get; set; }
    public bool Enable { get; set; }
    public virtual List<ReferralLinkTracking> ReferralLinkTrackings { get; set; }
}

