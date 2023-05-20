use master;
drop database if exists djecji_vrtic;
create database djecji_vrtic;
use djecji_vrtic;

create table djecji_vrtic(
	sifra int not null primary key identity(1,1),
	naziv varchar(50),
	adresa varchar(50)
);
create table odgojna_skupina(
	sifra int not null primary key identity(1,1),
	naziv varchar(50),
	djelatnica int,
	djecji_vrtic int,
	djeca int,
	datum_pocetka datetime,
);
create table djelatnica(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	oib char(11),
	strucna_sprema varchar(50)
);
create table djeca(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	odgojna_skupina int
);

alter table odgojna_skupina add foreign key (djelatnica) references djelatnica(sifra);
alter table djeca add foreign key (odgojna_skupina) references odgojna_skupina(sifra);
alter table odgojna_skupina add foreign key (djecji_vrtic) references djecji_vrtic(sifra);