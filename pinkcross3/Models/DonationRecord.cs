using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PinkCrossDonationRecord.Models
{
    public class DonationRecord
    {
        public string DonationRecord_id { get; set; }

        [Required(ErrorMessage = "Please enter the date of donation")]
        [Display(Name = "Date of Donation")]
        public DateTime DateOfDonation { get; set; }

        [Required(ErrorMessage = "Please enter the mode of donation ")]
        [Display(Name = "Mode of Donation")]
        public string ModeOfDonation { get; set; }

        [Required(ErrorMessage = "Please enter the donation amount")]
        [Display(Name = " Amount Donated")]
        public decimal DonationAmount { get; set; }

        [Required(ErrorMessage = "Please enter the purpose of donation")]
        [Display(Name = "Purpose of Donation")]
        public string PurposeOfDonation  { get; set; }

        [Required(ErrorMessage = "Indicate as Confirmed or Not Confirmed ")]
        [Display(Name = "Donation Type")]
        public string DonorType { get; set; }

        [Required(ErrorMessage = "Please enter the comapny name ")]
        [Display(Name = "companyname")]
        public string CompanyName { get; set; } 

        [Required(ErrorMessage = "Please enter the receipt number ")]
        [Display(Name = "receiptnumber")]
        public string ReceiptNumber { get; set; }

        [Required(ErrorMessage = "Please enter the date of receipt ")]
        [Display(Name = "dateofreceipt")]
        public DateTime DateOfReceipt { get; set; }

        [Required(ErrorMessage = "Please enter the tax exemption status ")]
        [Display(Name = "taxexemptionstatus")]
        public string TaxExemptionStatus{ get; set; }

        public int Donornric{ get; set; }
    }
}
