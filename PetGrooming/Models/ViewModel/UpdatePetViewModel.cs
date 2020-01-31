using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming.Models.ViewModel
{
    public class UpdatePetViewModel
    {
        //informatoin need to make update pet work
        //info about one pet
        //info about many species
        public Pet pet{get; set;}
        public List<Species> species { get; set; }
        //it only create relation between view and controller
    }
}