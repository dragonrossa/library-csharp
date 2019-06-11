using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Rezervacija
    {

       
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NaslovKnjige { get; set; }
     

        public Rezervacija(string ime, string prezime, string naslovKnjige)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.NaslovKnjige = naslovKnjige;
           
        }


    }
}