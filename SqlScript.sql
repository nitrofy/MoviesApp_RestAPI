	Create DATABASE MovieAPIProject
GO
	USE MovieAPIProject
GO
	Create Table Producers
	(
	ProducerId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ProducerName VARCHAR (60) NOT NULL Default '',
	Bio VARCHAR (150) NOT NULL Default '',
	DateOfBirth DATE NOT NULL Default '',
	Company Varchar(100) NOT NULL Default '',
	Gender varchar(6) NOT NULL Default ''
	)
GO
	Create Table Actors
	(
	ActorId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ActorName VARCHAR (60) NOT NULL Default '',
	Bio VARCHAR (150) NOT NULL Default '',
	DateOfBirth DATE NOT NULL Default '',
	Gender varchar(6) NOT NULL Default ''
	)
GO
	Create Table Movies
	(
	MovieId int identity(1,1) not null primary key,
	MovieName varchar(80) not null default '',
	ProducerId int not null Foreign Key references Producers(ProducerId),
	DateOfRelease DATE not null default getdate(),
	Plot varchar(200) not null default''
	)
GO
	Create Table MovieCast
	(
	SrNo int identity(1,1) not null primary key,
	MovieId int not null Foreign Key references Movies(MovieId),
	ActorId int not null Foreign Key references Actors(ActorId)
	)
GO
	Create TYPE TempArrayTable AS TABLE
	(
	ArrayItem int
	)
GO
	Create Table MoviePosters
	(
	SrNo int identity(1,1) not null primary key,
	MovieId int not null Foreign Key references Movies(MovieId),
	Poster VARCHAR(MAX) not null
	)
GO
	CREATE PROCEDURE sp_GetProducersListDetail
	AS
	BEGIN
		select ProducerId,ProducerName,Bio,Company,Gender,DateOfBirth as DOB from Producers
	END
GO
	Create PROCEDURE sp_GetProducersListShort
	AS
	BEGIN
		select ProducerId,ProducerName from Producers
	END
GO
	CREATE PROCEDURE sp_GetActorsListDetail
	AS
	BEGIN
		select ActorId,ActorName,Bio,Gender,DateOfBirth as DOB from Actors
	END
GO
	Create PROCEDURE sp_GetActorsListShort
	AS
	BEGIN
		select ActorId,ActorName from Actors
	END
GO
	Create Procedure sp_AddUpdateProducer  
	@ProducerId int = 0,  
	@ProducerName VARCHAR (60),  
	@Bio VARCHAR (150)='',  
	@DOB DATE='',  
	@Gender varchar(6)='',  
	@Company Varchar(100)='',  
	@Flag varchar(1)=''  
	AS  
	BEGIN  
	 IF (@Flag = 'C')  
	 BEGIN  
	  IF NOT EXISTS(SELECT DISTINCT ProducerId from Producers where ProducerName=@ProducerName)  
	   BEGIN  
		Insert into Producers(ProducerName,Bio,DateOfBirth,Gender,Company)  
		values ( @ProducerName,@Bio,@DOB,@Gender,@Company)   
		Select '0' AS Status,'Producer Added successfully' AS Message  
	   END  
	  else  
	   begin  
	   Select '1' AS Status,'Producer Name already exists' AS Message  
	   end  
	 END  
	 else IF (@Flag = 'U')  
	 BEGIN  
	  IF EXISTS(SELECT DISTINCT ProducerId from Producers where ProducerId = @ProducerId)  
	  BEGIN  
	   Update Producers  
		set  
		 ProducerName=@ProducerName,  
		 Bio=@Bio,  
		 DateOfBirth=@DOB,  
		 Gender=@Gender,  
		 Company=@Company  
		where ProducerId =@ProducerId  
	   Select '0' AS Status,'Producer Updated successfully' AS Message  
	  END  
	  else  
	  begin  
	   Select '1' AS Status,'ProducerId does not exist.' AS Message  
	  end  
  
	 END  
	 else IF (@Flag = 'D')  
	 BEGIN  
	  IF EXISTS(SELECT DISTINCT ProducerId from Producers where ProducerId = @ProducerId)  
	  BEGIN  
	   Delete from Producers where ProducerId =@ProducerId  
	   Select '0' AS Status,'Producer Deleted successfully' AS Message  
	  END  
	  else  
	  begin  
	   Select '1' AS Status,'ProducerId does not exist.' AS Message  
	  end  
	 END  
	 else  
	  begin  
	   Select '1' AS Status,'Invalid flag' AS Message  
	  end  
	END
GO
	Create Procedure sp_AddUpdateActor  
	@ActorId int = 0,  
	@ActorName VARCHAR (60),  
	@Bio VARCHAR (150)='',  
	@DOB DATE='',  
	@Gender varchar(6)='',  
	@Flag varchar(1)=''  
	AS  
	BEGIN  
	 IF (@Flag = 'C')  
	 BEGIN  
	  IF NOT EXISTS(SELECT DISTINCT ActorId from Actors where ActorName=@ActorName)  
	   BEGIN  
		Insert into Actors(ActorName,Bio,DateOfBirth,Gender)  
		values ( @ActorName,@Bio,@DOB,@Gender)   
		Select '0' AS Status,'Actor Added successfully' AS Message  
	   END  
	  else  
	   begin  
	   Select '1' AS Status,'Actor Name already exists' AS Message  
	   end  
	 END  
	 else IF (@Flag = 'U')  
	 BEGIN  
	  IF EXISTS(SELECT DISTINCT ActorId from Actors where ActorId = @ActorId)  
	  BEGIN  
	   Update Actors  
		set  
		 ActorName=@ActorName,  
		 Bio=@Bio,  
		 DateOfBirth=@DOB,  
		 Gender=@Gender  
		where ActorId =@ActorId  
	   Select '0' AS Status,'Actor Updated successfully' AS Message  
	  END  
	  else  
	  begin  
	   Select '1' AS Status,'ActorId does not exist.' AS Message  
	  end  
  
	 END  
	 else IF (@Flag = 'D')  
	 BEGIN  
	  IF EXISTS(SELECT DISTINCT ActorId from Actors where ActorId = @ActorId)  
	  BEGIN  
	   Delete from Actors where ActorId =@ActorId  
	   Select '0' AS Status,'Actor Deleted successfully' AS Message  
	  END  
	  else  
	  begin  
	   Select '1' AS Status,'ActorId does not exist.' AS Message  
	  end  
	 END  
	 else  
	  begin  
	   Select '1' AS Status,'Invalid flag' AS Message  
	  end  
	END
GO
	Create PROCEDURE sp_GetMoviesListShort
	AS
	BEGIN
		select MovieId,MovieName from Movies
	END
GO
	CREATE PROCEDURE sp_GetMoviesListDetail
	AS
	BEGIN
		SELECT MovieId,MovieName,DateOfRelease,Plot,P.ProducerId,P.ProducerName
		FROM Movies M
		LEFT JOIN Producers P on M.ProducerId=P.ProducerId

		SELECT MovieId,MC.ActorId,A.ActorName 
		FROM MovieCast MC
		LEFT JOIN Actors A on MC.ActorId=A.ActorId 

	END
GO
	CREATE Procedure sp_AddUpdateMovie
	@MovieId int ,
	@MovieName varchar(80) ='',
	@ProducerId int = 0,
	@DateOfRelease DATE ='',
	@Plot varchar(200)='',
	@ActorIds [dbo].[TempArrayTable] READONLY,
	@Flag varchar(1)=''
	AS

	BEGIN
		IF (@Flag = 'C')
		BEGIN
			IF NOT EXISTS(SELECT DISTINCT MovieId from Movies where MovieName=@MovieName)
				BEGIN
					IF exists(select top 1 ArrayItem from @ActorIds where ArrayItem not in (select ActorId from Actors) )
					begin 
						Select '1' AS Status,'Invalid ActorId, does not exist' AS Message
					end
					else IF not exists(select top 1 ProducerId from Producers where ProducerId = @ProducerId )
					begin 
						Select '1' AS Status,'Invalid ProducerId, does not exist' AS Message
					end
					else
					begin
						Insert into Movies(MovieName,Plot,DateOfRelease,ProducerId)
						values ( @MovieName,@Plot,@DateOfRelease,@ProducerId) 
				
						select @MovieId=MovieId from Movies where MovieName=@MovieName and DateOfRelease=@DateOfRelease
				
						Delete from MovieCast where MovieId=@MovieId
						if exists(select top 1 ArrayItem from @ActorIds)
							begin
							insert into MovieCast(MovieId,ActorId)
							select @MovieId,ArrayItem from @ActorIds
							end
						
						Select '0' AS Status,'Movie Added successfully' AS Message
					end
				END
			else
				begin
				Select '1' AS Status,'Movie Name already exists' AS Message
				end
		END
		else IF (@Flag = 'U')
		BEGIN
			IF EXISTS(SELECT DISTINCT MovieId from Movies where MovieId = @MovieId)
			BEGIN
				IF exists(select top 1 ArrayItem from @ActorIds where ArrayItem not in (select ActorId from Actors) )
					begin 
						Select '1' AS Status,'Invalid ActorId, does not exist' AS Message
					end
				else IF not exists(select top 1 ProducerId from Producers where ProducerId = @ProducerId )
					begin 
						Select '1' AS Status,'Invalid ProducerId, does not exist' AS Message
					end
				else
					begin 
				
						Update Movies
							set
								MovieName=@MovieName,
								Plot=@Plot,
								DateOfRelease=@DateOfRelease,
								ProducerId=@ProducerId
							where MovieId =@MovieId
			
						Delete from MovieCast where MovieId=@MovieId
						if exists(select top 1 ArrayItem from @ActorIds)
							begin
							insert into MovieCast(MovieId,ActorId)
							select @MovieId,ArrayItem from @ActorIds
							end
						
						Select '0' AS Status,'Movie Updated successfully' AS Message
					END
			END
			else
			begin
				Select '1' AS Status,'MovieId does not exist.' AS Message
			end

		END
		else IF (@Flag = 'D')
		BEGIN
			IF EXISTS(SELECT DISTINCT MovieId from Movies where MovieId = @MovieId)
			BEGIN
				Delete from MovieCast where MovieId=@MovieId
				Delete from MoviePosters where MovieId=@MovieId
				Delete from Movies where MovieId = @MovieId
				Select '0' AS Status,'Movie Deleted successfully' AS Message
			END
			else
			begin
				Select '1' AS Status,'MovieId does not exist.' AS Message
			end
		END
		else
			begin
				Select '1' AS Status,'Invalid flag' AS Message
			end
	END
GO
CREATE Procedure sp_AddUpdatePoster
	@MovieId int ,
	@Poster VARCHAR(MAX),
	@Flag varchar(1)=''
	AS

	BEGIN
		IF (@Flag = 'R')--Read / Fetch
		BEGIN
			IF EXISTS(SELECT DISTINCT MovieId from MoviePosters where MovieId=@MovieId)
				BEGIN
					
						Select '2' AS Status,MovieId,Poster from MoviePosters 				
				END
			else
				begin
				Select '1' AS Status,'Movie Poster does not exist for this MovieId.' AS Message
				end
			END
			else IF (@Flag = 'C')
		BEGIN
			IF NOT EXISTS(SELECT DISTINCT MovieId from MoviePosters where MovieId=@MovieId)
				BEGIN
					
						Delete from MoviePosters where MovieId=@MovieId
						if (@Poster <> '' and not exists(select srno from MoviePosters where MovieId=@MovieId))
						begin
							insert into MoviePosters(MovieId,Poster)
							Values (@MovieId,@Poster)
							Select '0' AS Status,'Movie Poster Added successfully' AS Message
						end					
				END
			else
				begin
				Select '1' AS Status,'Movie Poster Already exists for this MovieId.' AS Message
				end
			END
		else IF (@Flag = 'U')
		BEGIN
			IF EXISTS(SELECT DISTINCT MovieId from MoviePosters where MovieId = @MovieId)
			BEGIN
							
				Update MoviePosters Set
				Poster=@Poster
				Where MovieId = @MovieId

				Select '0' AS Status,'Movie Poster Updated successfully' AS Message
					
			END
			else
			begin
				Select '1' AS Status,'Movie Poster does not exist for this MovieId.' AS Message
			end

		END
		else IF (@Flag = 'D')
		BEGIN
			IF EXISTS(SELECT DISTINCT MovieId from MoviePosters where MovieId = @MovieId)
			BEGIN
				Delete from MoviePosters where MovieId=@MovieId
				Select '0' AS Status,'Movie Poster Deleted successfully' AS Message
			END
			else
			begin
				Select '1' AS Status,'Movie Poster does not exist for this MovieId.' AS Message
			end
		END
		else
			begin
				Select '1' AS Status,'Invalid flag' AS Message
			end
	END
GO


