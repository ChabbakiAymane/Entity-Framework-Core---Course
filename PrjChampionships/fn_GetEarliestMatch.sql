CREATE FUNCTION [dbo].[GetEarliestMatch] (@teamId int)
	RETURNS datetime
	BEGIN
		DECLARE @result datetime
		SELECT TOP 1 @result = date
		FROM MATCHES [dbo].[Matches]
		ORDER BY Date
		RETURN @result
	END
END