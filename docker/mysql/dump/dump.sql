CREATE TABLE Users (
    PersonID int NOT NULL AUTO_INCREMENT,
    UserName VARCHAR(255),
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    Gender VARCHAR(255),
    PhoneNr VARCHAR(20),
    Place VARCHAR(255),
    Email VARCHAR(255),
    FieldOfWork VARCHAR(255),
    Education VARCHAR(255),
    Experience VARCHAR(255),
    DesiredFunction VARCHAR(255),
    Motivation VARCHAR(255),
    Location VARCHAR(255),
    Password VARCHAR(255),
    PRIMARY KEY (PersonID)
);