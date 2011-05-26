SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Create function [dbo].DecToBase
(
@val as BigInt,
@base as int
)
returns varchar(63)
as
Begin
	If (@val<0) OR (@base < 2) OR (@base> 36) 
		Return Null;
	Declare @answer as varchar(63) = '';
	Declare @alldigits as varchar(36) = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'
	While @val>0
	Begin
		Set @answer=Substring(@alldigits,@val % @base + 1,1) + @answer;
		Set @val = @val / @base;
	End
	return @answer;
End
GO
