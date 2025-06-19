using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class CourtSession
    {
        [Key]
        public int Id { get; set; } 
        public int Code {  get; set; }
        public DateOnly CourtSessionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int IssueFileId {  get; set; }
        public string? IssueNumber { get; set; }
        public string IssueName { get; set; }
        public DateOnly DateNow { get; set; } 
        /// <summary>
        /// client properties
        /// </summary>
        public int ClientId { get; set; }

        public string? ClientProperty { get; set; }
        public string? ClientPhoneNumber { get; set; }
        public string? ClientAddress { get; set; }
        /// <summary>
        /// Party properties 
        /// </summary>
        public int PartyId { get; set; }


        public string? PartyPhoneNumber { get; set; }
        public string? PartyAddress { get; set; }
        public string? PartyWorkPlace { get; set; }
        /// <summary>
        /// Issuetypes properties 
        /// </summary>
        public int IssueTypeId { get; set; }

        public decimal IssueValueFees { get; set; } = 0;
        public string? IssueDescription { get; set; }
        public string? IssueDegreeNegotiation { get; set; }
        /// <summary>
        /// Anthors properties 
        /// </summary>
        public int CourtId { get; set; }
        public string? ClaimNumber { get; set; }
        public int? YearOfIssue { get; set; }
        public int CenterId { get; set; }
        public string? RuleOfIssue { get; set; }
        public DateOnly? DateNextSession { get; set; }
        public string? WhatHappenedInTheCourtSession { get; set; }
        public byte[]? IssueImage { get; set; }
    }
}
