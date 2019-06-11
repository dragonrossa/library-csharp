CREATE DATABASE knjiznica;
use knjiznica


CREATE TABLE Clanovi (
ClanID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
Ime varchar(50) NOT NULL,
Prezime varchar(50) NOT NULL,
AdresaStanovanja varchar(100),
MjestoStanovanja varchar(100),
Email varchar(50),
DatumRodjenja date,
DatumUclanjenja date
)

//naknadno smo dodali polje Telefon, varchar(50) zbog upisa novog clana kroz aplikaciju

drop table Clanovi

CREATE TABLE Knjige(
KnjigaID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
NaslovKnjige varchar(200) NOT NULL,
ImePisca varchar(50),
PrezimePisca varchar(50),
GodinaIzdanja varchar(4)
)

select * from Knjige

drop table Knjige

CREATE TABLE Posudba (
PosudbaID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
ClanID int FOREIGN KEY REFERENCES Clanovi(ClanID) NOT NULL,
KnjigaID int FOREIGN KEY REFERENCES Knjige(KnjigaID) NOT NULL,
DatumPosudbe DATETIME DEFAULT CURRENT_TIMESTAMP,
DatumVracanja date,
)

drop table Posudba


CREATE TABLE Zaposlenici (
KorisnickoIme varchar(50),
Lozinka varchar(50)
)


INSERT INTO Zaposlenici VALUES('rodjuga@gmail.com', '1D3d+891s')
INSERT INTO Zaposlenici VALUES('rosana', 'abcd123')

select * from Zaposlenici



select * from Clanovi
select * from Knjige
select * from Posudba

DELETE  FROM Knjige
DELETE FROM Clanovi


--INSERT--


INSERT INTO Clanovi VALUES ('Filip', 'Ruzić', 'Stubicka ulica 5','Zagreb', 'filip.ruzic@gmail.com', '1992-10-28', '2005-05-03')
INSERT INTO Clanovi VALUES ('Marija', 'Kiš', 'Ilica 243','Zagreb', 'marija.kis@yahoo.com', '1987-11-12', '2005-09-04')
INSERT INTO Clanovi VALUES ('Ivan', 'Vranić', 'Čakovecka 23','Karlovac', 'ivranic@gmail.com', '1983-01-23', '2005-11-15')
INSERT INTO Clanovi VALUES ('Monika', 'Grabar', 'Teslina 14','Osijek', 'monikicag@gmail.com', '1995-06-11', '2011-03-09')
INSERT INTO Clanovi VALUES ('Ana', 'Cazin', 'Bolnička cesta 1','Sisak', 'anacazing@gmail.com', '1975-01-11', '2010-10-31')
INSERT INTO Clanovi VALUES ('Lovro', 'Horvat', 'Samoborska cesta 11','Osijek', 'lhorvatg@gmail.com', '1964-04-12', '2009-09-30')
INSERT INTO Clanovi VALUES ('Matija', 'Gužvinec', 'Varaždinska ulica 154','Pula', 'matijaguz@gmail.com', '1988-06-18', '2004-07-24')
INSERT INTO Clanovi VALUES ('Stjepan', 'Radoš', 'Vinkovačka ulica 25','Rijeka', 'srados@gmail.com', '1997-03-08', '2005-06-04')
INSERT INTO Clanovi VALUES ('Ivica', 'Barun', 'Maksimirska cesta 46','Dubrovnik', 'ivica.barun@gmail.com', '1961-02-02', '2006-05-10')
INSERT INTO Clanovi VALUES ('Pero', 'Đuga', 'Horvaćanska cesta 32','Zagreb', 'djperica@gmail.com', '1994-11-26', '2007-04-14')
INSERT INTO Clanovi VALUES ('Magdalena','Lovrić', 'Zagrebačka cesta 55','Zagreb', 'malovric@gmail.com', '1952-01-12', '2001-01-10')
INSERT INTO Clanovi VALUES ('Lucija','Lovrić', 'Zagrebačka cesta 55','Zagreb', 'malovric@gmail.com', '1952-01-12', '2001-01-10')

INSERT INTO Knjige VALUES ('Sto godina samoće','Gabriela Garcia', 'Marquez', '1967')
INSERT INTO Knjige VALUES ('Ana Karenjina', 'Lav Nikolajević', 'Tolstoj', '1877')
INSERT INTO Knjige VALUES ('Mali princ', 'Antoine', 'de Saint Exupery', '1943')
INSERT INTO Knjige VALUES ('Stranac', 'Albert', 'Camus', '1942')
INSERT INTO Knjige VALUES ('Zločin i kazna', 'Fjodor', 'Mihajlovič Dostojevski', '1866')
INSERT INTO Knjige VALUES ('Istočno od raja', 'John ', 'Steinbeck', '1952')
INSERT INTO Knjige VALUES ('Životinjska farma', 'George', 'Orwell', '1945')
INSERT INTO Knjige VALUES ('Ubiti pticu rugalicu', 'Harper', 'Lee', '1960')
INSERT INTO Knjige VALUES ('Novela o šahu', 'Stefan', 'Zweig', '1941')
INSERT INTO Knjige VALUES ('Posljednje predavanje', 'Randy', 'Pausch', '2008')


INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (3,'Mali princ')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (4,'Ana Karenjina')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (5,'Sto godina samoce')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (6,'Novela o šahu')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (7,'Posljednje predavanje')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (8,'Stranac')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (9,'Ubiti pticu rugalicu')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (10,'Zlocin i kazna')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (12,'Životinjska farma')
INSERT INTO Posudba(ClanID,NaslovKnjige) VALUES (13,'Istocno od raja')


INSERT INTO Posudba(ClanID, KnjigaID) VALUES (1,6)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (2,4)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (3,3)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (4,2)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (5,1)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (6,5)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (7,7)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (8,8)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (9,10)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (10,9)
INSERT INTO Posudba(ClanID, KnjigaID) VALUES (10,9)

------



SELECT * FROM Posudba

--- CREATE VIEW
---Pogled koji vraća popis svih slobodnih knjiga u knjižnici za posudbu;

INSERT INTO Knjige VALUES ('Svijet se raspada','Chinua', 'Achebe', '1958')
select * from Knjige

CREATE VIEW free_books AS
SELECT k.KnjigaID, k.NaslovKnjige, k.ImePisca, k.PrezimePisca, k.GodinaIzdanja
FROM  Knjige k
WHERE NOT EXISTS (SELECT p.PosudbaID FROM Posudba p WHERE k.KnjigaID = p.KnjigaID);

---ispis iz tog pogleda
SELECT * FROM [free_books]; 

---Pohranjenu proceduru koja kreira novu tablicu naziva NEAKTIVNI i u nju kopira
---sve članove koji nisu nikad posudili ni jednu knjigu, a zatim iz tablice ČLANOVI
---te iste članove briše;

CREATE TABLE Clanovi (
ClanID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
Ime varchar(50) NOT NULL,
Prezime varchar(50) NOT NULL,
AdresaStanovanja varchar(100),
MjestoStanovanja varchar(100),
Email varchar(50),
DatumRodjenja date,
DatumUclanjenja date
)

go
CREATE PROCEDURE neaktivni_sp AS
    IF object_id('dbo.Neaktivni') IS NOT NULL
         DROP TABLE Neaktivni
    CREATE TABLE Neaktivni(
		ClanID int PRIMARY KEY NOT NULL,
		Ime varchar(50) NOT NULL,
		Prezime varchar(50) NOT NULL,
		AdresaStanovanja varchar(100),
		MjestoStanovanja varchar(100),
		Email varchar(50),
		DatumRodjenja date,
		DatumUclanjenja date
	)
    INSERT INTO Neaktivni(ClanID, Ime, Prezime, AdresaStanovanja, MjestoStanovanja, Email, DatumRodjenja, DatumUclanjenja) 
	SELECT * FROM Clanovi c WHERE NOT EXISTS (SELECT p.ClanID FROM Posudba p WHERE c.ClanID = p.ClanID)
	DELETE Clanovi FROM Clanovi JOIN Neaktivni ON Neaktivni.ClanID = Clanovi.ClanID 
go

exec neaktivni_sp;

--Okidač koji ne dozvoljava brisanje podataka iz tablice Posudbe;

create trigger okidac
delete
on [table_name]  
[for each row]  
[trigger_body] 


create trigger saftey  
on Posudba  
for  
delete 
as  
print'no delete for you'  
rollback; 

--test
delete from Posudba

--SQL korisnika Ivicu (korisničko ime: ivica, lozinka: Pa$$w0rd) te mu dodajte rolu db_datawriter;

--Security -> Logins -> Ivica

