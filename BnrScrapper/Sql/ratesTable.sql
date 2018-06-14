Create Table Rates(
    RateDate DATE not NULL,
    Robid3M DECIMAL,
    Robid6M DECIMAL,
	Robid9M DECIMAL,
    Robid12M DECIMAL,
    Robor3M DECIMAL,
    Robor6M DECIMAL,
    Robor9M DECIMAL,
    Robor12M DECIMAL
    CONSTRAINT PK_Rates Primary KEY(RateDate)
);