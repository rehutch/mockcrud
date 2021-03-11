CREATE TABLE [dbo].[Books] (
    [BookId]     INT        NOT NULL,
    [Title]      NCHAR (10) NOT NULL,
    [Genre]      NCHAR (10) NOT NULL,
    [CheckedOut] BIT        NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([BookId] ASC)
);