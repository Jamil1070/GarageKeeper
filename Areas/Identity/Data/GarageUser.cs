using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Garage2User class


//[PersonalData]
//[Column(TypeName = "nvarchar(100)")]
//public string? Admincode { get; set; }

//[PersonalData]
//public bool IsAdmin { get; set; }

//[PersonalData]
//[Column(TypeName = "nvarchar(100)")]



public class GarageUser : IdentityUser
{

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Surname { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Adress { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Postcode { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? City { get; set; }

    [PersonalData]
    public DateTime? DateOfBirth { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Admincode { get; set; }

    [PersonalData]
    public bool IsAdmin { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string ConfirmEmail { get; set; }
}

