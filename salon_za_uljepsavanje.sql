use master;
drop database if exists salon_za_uljepsavanje;
go

create database salon_za_uljepsavanje;
go
use salon_za_uljepsavanje;

create table salon_za_uljepsavanje(
	sifra int not null primary key identity(1,1),
	adresa varchar(50),
	ime varchar(50),
);
create table djelatnica(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	oib char(11),
	salon_za_uljepsavanje int,
	usluga int
);
create table korisnik(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	spol bit,
	djelatnica int,
	naziv int,
	usluga int
);

create table usluga(
	sifra int not null primary key identity(1,1),
	naziv varchar(50),
	trajanje datetime
);

alter table djelatnica add foreign key (salon_za_uljepsavanje) references salon_za_uljepsavanje(sifra);
alter table korisnik add foreign key (usluga) references usluga(sifra);
alter table korisnik add foreign key (usluga) references djelatnica(sifra);