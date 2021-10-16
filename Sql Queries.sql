select * from cities;

CREATE TABLE States (
    ID int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    StateName varchar(255)
);

insert into states (stateName)
select distinct state from cities;

select * from states;


CREATE TABLE ZipCodes (
    ID int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    ZipCode varchar(5) );
    
    
insert into ZipCodes (ZipCode)
select distinct Zip from cities;
products


