using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Rezervacija
    {
        public int RezervacijaID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NaslovKnjige { get; set; }
        public string Seminar { get; set; }
        public string Status { get; set; }
        public int PosudbaID { get; set; }

        public Rezervacija()
        {

        }

        public Rezervacija(int rezervacijaID, string ime, string prezime, string naslovKnjige, int posudbaID)
        {
            this.RezervacijaID = rezervacijaID;
            this.Ime = ime;
            this.Prezime = prezime;
            this.NaslovKnjige = naslovKnjige;
            this.PosudbaID = posudbaID;
        }

        public Rezervacija(int rezervacijaID, string ime, string prezime, string naslovKnjige, string seminar, string status, int posudbaID)
        {
            this.RezervacijaID = rezervacijaID;
            this.Ime = ime;
            this.Prezime = prezime;
            this.NaslovKnjige = naslovKnjige;
            this.Seminar = seminar;
            this.Status = status;
            this.PosudbaID = posudbaID;
        }


        public Rezervacija(string ime, string prezime, string naslovKnjige, string seminar, string status)
        {
            
            this.Ime = ime;
            this.Prezime = prezime;
            this.NaslovKnjige = naslovKnjige;
            this.Seminar = seminar;
            this.Status = status;
        }



    }
}