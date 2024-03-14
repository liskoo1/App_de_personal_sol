USE [master]
GO
/****** Object:  Database [SpaceJamp]    Script Date: 05/03/2024 18:24:19 ******/
CREATE DATABASE reve
GO
ALTER DATABASE reve SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC reve.[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE reve SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE reve SET ANSI_NULLS OFF 
GO
ALTER DATABASE reve SET ANSI_PADDING OFF 
GO
ALTER DATABASE reve SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE reve SET ARITHABORT OFF 
GO
ALTER DATABASE reve SET AUTO_CLOSE OFF 
GO
ALTER DATABASE reve SET AUTO_SHRINK OFF 
GO
ALTER DATABASE reve SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE reve SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE reve SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE reve SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE reve SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE reve SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE reve SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE reve SET  DISABLE_BROKER 
GO
ALTER DATABASE reve SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE reve SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE reve SET TRUSTWORTHY OFF 
GO
ALTER DATABASE reve SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE reve SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE reve SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE reve SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE reve SET RECOVERY SIMPLE 
GO
ALTER DATABASE reve SET  MULTI_USER 
GO
ALTER DATABASE reve SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE reve SET DB_CHAINING OFF 
GO
ALTER DATABASE reve SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE reve SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE reve SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE reve SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE reve SET QUERY_STORE = ON
GO
ALTER DATABASE reve SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE reve
GO
/****** Object:  Table [dbo].[Asistencia]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asistencia](
	[Id_asistencia] [int] IDENTITY(1,1) NOT NULL,
	[Id_Personal] [int] NULL,
	[Fecha_Entrada] [datetime] NULL,
	[Fecha_Salida] [datetime] NULL,
	[Horas] [decimal](18, 2) NULL,
	[ObservacionesEntrada] [varchar](3800) NULL,
	[ObservacionesSalida] [varchar](3800) NULL,
	[Firma] [varbinary](max) NULL,
 CONSTRAINT [PK_Asistencia_1] PRIMARY KEY CLUSTERED 
(
	[Id_asistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cargos]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cargos](
	[Id_cargo] [int] IDENTITY(1,1) NOT NULL,
	[Cargo] [varchar](50) NULL,
	[SueldoPorHora] [numeric](18, 2) NULL,
 CONSTRAINT [PK_Cargos] PRIMARY KEY CLUSTERED 
(
	[Id_cargo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[Id_Empresa] [int] IDENTITY(1,1) NOT NULL,
	[CIF] [varchar](10) NULL,
	[NombreEmpresa] [varchar](100) NULL,
	[Direccion] [varchar](300) NULL,
	[TelefonoEmpresa] [int] NULL,
	[EmailEmpresa] [varchar](100) NULL,
 CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED 
(
	[Id_Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Imagenes]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Imagenes](
	[Id_imagen] [int] IDENTITY(1,1) NOT NULL,
	[Id_Personal] [int] NULL,
	[imagen] [varbinary](max) NULL,
 CONSTRAINT [PK_Imagenes] PRIMARY KEY CLUSTERED 
(
	[Id_imagen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personal](
	[Id_Personal] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[Identificacion] [varchar](255) NOT NULL,
	[Pais] [varchar](255) NOT NULL,
	[Id_cargo] [int] NOT NULL,
	[SueldoPorHora] [numeric](18, 2) NOT NULL,
	[Estado] [varchar](255) NOT NULL,
	[Codigo] [varchar](255) NOT NULL,
	[Id_Empresa] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Personal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regalos]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regalos](
	[Id_Regalo] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](500) NULL,
	[CodigoArticulo] [int] NULL,
	[Cantidad] [int] NULL,
 CONSTRAINT [PK_Regalos] PRIMARY KEY CLUSTERED 
(
	[Id_Regalo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TicketsPendientes]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketsPendientes](
	[Id_Ticket] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [int] NULL,
	[Id_Personal] [int] NULL,
	[NumeroOperacion] [int] NULL,
	[NombreDeudor] [varchar](300) NULL,
	[IdentificacionDeudor] [varchar](10) NULL,
	[Matricula] [varchar](10) NULL,
	[Telefono] [int] NULL,
	[DireccionDeudor] [varchar](255) NULL,
	[EmailDeudor] [varchar](255) NULL,
	[CuantiaDeuda] [decimal](18, 2) NULL,
	[FechaDeDeuda] [datetime] NULL,
	[FechaDePago] [datetime] NULL,
	[EstadoDeuda] [varchar](10) NULL,
	[Firma] [varbinary](max) NULL,
	[Observaciones] [varchar](3800) NULL,
 CONSTRAINT [PK_TicketsPendientes] PRIMARY KEY CLUSTERED 
(
	[Id_Ticket] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id_Personal] [int] NULL,
	[NameUser] [varchar](255) NULL,
	[Pass] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asistencia]  WITH CHECK ADD  CONSTRAINT [FK_Asistencia_Personal] FOREIGN KEY([Id_Personal])
REFERENCES [dbo].[Personal] ([Id_Personal])
GO
ALTER TABLE [dbo].[Asistencia] CHECK CONSTRAINT [FK_Asistencia_Personal]
GO
ALTER TABLE [dbo].[Personal]  WITH CHECK ADD  CONSTRAINT [FK_Personal_Cargos] FOREIGN KEY([Id_cargo])
REFERENCES [dbo].[Cargos] ([Id_cargo])
GO
ALTER TABLE [dbo].[Personal] CHECK CONSTRAINT [FK_Personal_Cargos]
GO
/****** Object:  StoredProcedure [dbo].[ActualizarCargo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ActualizarCargo]
@Id_Personal INT,
@Id_cargo INT,
@SueldoPorHora DECIMAL(18,2)
AS
UPDATE Personal set Id_cargo = @Id_cargo where Id_Personal = @Id_Personal;
UPDATE Personal set SueldoPorHora = @SueldoPorHora where Id_Personal = @Id_Personal;
RETURN 0;
GO
/****** Object:  StoredProcedure [dbo].[ArticuloMasUno]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ArticuloMasUno]
@Descripcion varchar(500)
as
Update Regalos SET Cantidad = Cantidad + 1 where Descripcion = @Descripcion
GO
/****** Object:  StoredProcedure [dbo].[ArticuloMenosUno]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ArticuloMenosUno]
@Descripcion varchar(500)
as
if (SELECT Cantidad from Regalos  where Descripcion = @Descripcion) = 0
Raiserror('En base de datos hay 0 cantidad de articulos con ese codigo',18,2)
else

Update Regalos SET Cantidad = Cantidad - 1 where Descripcion = @Descripcion
GO
/****** Object:  StoredProcedure [dbo].[Buscar_Cargos]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Buscar_Cargos]
	@Buscador varchar(50)
AS
	SELECT * from cargos where Cargo like '%' + @Buscador + '%'
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[BuscarIdPersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[BuscarIdPersonal]
@Identificacion varchar(255)
as
if NOT EXISTS(select Identificacion from Personal where Identificacion = @Identificacion)
raiserror('La IdentificaciÃ³n introducida no pertenece a ningun empleado',16,1)
else
Select Id_Personal,Nombre,Estado from Personal where Identificacion = @Identificacion;
GO
/****** Object:  StoredProcedure [dbo].[BuscarPersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BuscarPersonal]
	@Buscador varchar(50)
AS
Set Nocount On;
SELECT Id_Personal,Nombre,Identificacion,Pais,p.SueldoPorHora,Estado,Codigo,c.Id_cargo,c.Cargo from Personal as p
inner join Cargos as c on p.Id_cargo = c.Id_cargo
where Nombre like   @Buscador + '%';
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[BuscarPersonalYAsistencia]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BuscarPersonalYAsistencia]
	@Buscador varchar(50)
AS
Set Nocount On;
SELECT p.Id_Personal,p.Nombre,p.Identificacion,a.Fecha_Entrada,a.Fecha_Salida,a.Horas,a.ObservacionesEntrada,a.ObservacionesSalida,a.Firma
from Personal as p
inner join Asistencia as a on p.Id_Personal = a.Id_Personal
where Nombre like   @Buscador + '%';
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[BuscarRegaloCodigo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[BuscarRegaloCodigo]
@CodigoArticulo int
as
Select * from Regalos where CodigoArticulo = @CodigoArticulo
GO
/****** Object:  StoredProcedure [dbo].[BuscarTicketId]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[BuscarTicketId]
@Id_Ticket int
as
Select * from TicketsPendientes where Id_Ticket = @Id_Ticket;
GO
/****** Object:  StoredProcedure [dbo].[BuscarTicketPorNombre]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[BuscarTicketPorNombre]
@NombreDeudor varchar(300)
as
Select Id_Ticket,NumeroOperacion,NombreDeudor,Telefono,CuantiaDeuda,FechaDeDeuda,EstadoDeuda from TicketsPendientes where NombreDeudor like '%'+ @NombreDeudor + '%';
GO
/****** Object:  StoredProcedure [dbo].[BuscarUser]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BuscarUser]
	@NameUser varchar(255),
	@Pass varchar(255)
AS
SELECT Id_Personal,NameUser,Pass from Users
where NameUser = @NameUser and Pass = @Pass;
RETURN 1
GO
/****** Object:  StoredProcedure [dbo].[EliminarArticulo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[EliminarArticulo]
@CodigoArticulo int
as
if NOT EXISTS(SELECT CodigoArticulo from Regalos WHERE CodigoArticulo = @CodigoArticulo)
RAISERROR('Ese codigo no pertenece a ningun articulo existente',18,2)
else
Delete from Regalos WHERE CodigoArticulo = @CodigoArticulo
GO
/****** Object:  StoredProcedure [dbo].[EliminarPersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EliminarPersonal]
	@Id_Personal int
	
AS
	Update Personal set Estado = 'ELIMINADO' where Id_Personal = @Id_Personal
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[Insertar_Cargo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insertar_Cargo]
	@Cargo varchar(50),
	@SueldoPorHora numeric(18,2)
AS
if exists(select Cargo from Cargos where Cargo = @Cargo)
raiserror('El cargo ya existe',16,1)
else
	insert into Cargos values (@Cargo,@SueldoPorHora)
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[InsertarArticulo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[InsertarArticulo]
@Descripcion varchar(500),
@CodigoArticulo int,
@Cantidad int
as
if EXISTS(SELECT CodigoArticulo from Regalos WHERE CodigoArticulo = @CodigoArticulo)
RAISERROR('El codigo del articulo introducido esta relacionado con un articulo ya existente',18,2)
else
insert into Regalos values(@Descripcion,@CodigoArticulo,@Cantidad)
GO
/****** Object:  StoredProcedure [dbo].[InsertarAsistencia]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertarAsistencia]
	@Id_Personal int,
	@Fecha_Entrada DateTime,
	@Horas decimal(18,2),
	@ObservacionesEntrada varchar(3800),
	@Firma varBinary(MAX)

AS
if EXISTS(select Fecha_Salida from Asistencia where Id_Personal = @Id_Personal and Fecha_Salida IS NULL)
Raiserror('No puedes introducir otra entrada ya que tiene una entrada abierta. Cierra la anterior para volve a introducir otra entrada',16,2)
else if EXISTS(Select * from Personal WHERE Id_Personal = @Id_Personal And Estado != 'ACTIVO')
RAISERROR('Este usuario esta inactivo',16,2)
ELSE
Insert into Asistencia (Id_Personal,Fecha_Entrada,Horas,ObservacionesEntrada,Firma) values(@Id_Personal,@Fecha_Entrada,@Horas,@ObservacionesEntrada,@Firma) 
Return 0;
GO
/****** Object:  StoredProcedure [dbo].[InsertarEmpresa]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[InsertarEmpresa]
@Cif varchar(10),
@Nombre varchar(255),
@Direccion varchar(300),
@Telefono int,
@Email varchar(100)
as 
Insert into Empresa values(@Cif,@Nombre,@Direccion,@Telefono,@Email)
GO
/****** Object:  StoredProcedure [dbo].[InsertarPersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[InsertarPersonal]
   
    @Nombre VARCHAR(255),
    @Identificacion VARCHAR(255),
    @Pais VARCHAR(255),
    @Id_cargo INT ,
    @SueldoPorHora NUMERIC(18,2),
	@Id_Empresa int
	AS
	declare @Estado varchar(50)
	declare @Codigo varchar(50)
	declare @Id_personal int
	set @Estado='ACTIVO'
	set @Codigo = '-'
	if EXISTS(SELECT Identificacion FROM Personal WHERE Identificacion = @Identificacion)
	Raiserror('Ya existe un registro con esa identificaciÃ³n',16,1)
	Else 
	INSERT INTO Personal 
	values (@Nombre,@Identificacion,@Pais,@Id_cargo,@SueldoPorHora,@Estado,@Codigo,@Id_Empresa)
	Select @Id_personal = SCOPE_IDENTITY()
	update Personal set Codigo = @Id_personal where Id_Personal = @Id_personal

GO
/****** Object:  StoredProcedure [dbo].[InsertarTicket]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[InsertarTicket]
@Id_Empresa int,
@Id_Personal int,
@NumeroOperacion int,
@NombreDeudor varchar(300),
@IdentificacionDeudor varchar(10),
@Matricula varchar(10),
@Telefono int,
@DireccionDeudor varchar(255),
@EmailDeudor varchar(255),
@CuantiaDeuda decimal(18,2),
@FechaDeDeuda datetime,
@EstadoDeuda varchar(10),
@Firma varbinary(max),
@Observaciones varchar(3800)
as
declare @FechaDePago datetime
set @FechaDePago = '2000-01-01 00:00:00'
INSERT INTO TicketsPendientes
           ([Id_Empresa]
           ,[Id_Personal]
           ,[NumeroOperacion]
           ,[NombreDeudor]
           ,[IdentificacionDeudor]
           ,[Matricula]
           ,[Telefono]
           ,[DireccionDeudor]
           ,[EmailDeudor]
           ,[CuantiaDeuda]
           ,[FechaDeDeuda]
		   ,[FechaDePago]
           ,[EstadoDeuda]
           ,[Firma]
		   ,[Observaciones])
     VALUES
           (@Id_Empresa,
           @Id_Personal,
           @NumeroOperacion,
           @NombreDeudor,
           @IdentificacionDeudor,
           @Matricula,
           @Telefono,
           @DireccionDeudor,
           @EmailDeudor,
           @CuantiaDeuda,
           @FechaDeDeuda,
		   @FechaDePago,
           @EstadoDeuda,
           @Firma,
		   @Observaciones
		   )



GO
/****** Object:  StoredProcedure [dbo].[InsertarUser]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertarUser]
	@Id_Personal int,
	@NameUser varchar(255),
	@Pass varchar(255)
AS
if EXISTS(SELECT NameUser FROM Users WHERE NameUser = @NameUser)
Raiserror('Este nombre de usuario ya esta ocupado introduce otro nombre de usuario',16,1);
else
insert into Users values (@Id_Personal, @NameUser,@Pass)
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[ModificarArticulo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[ModificarArticulo]
@Id_Regalo int,
@Descripcion varchar(500),
@CodigoArticulo int,
@Cantidad int
as
Update Regalos set Descripcion = @Descripcion, CodigoArticulo = @CodigoArticulo,Cantidad = @Cantidad where Id_Regalo =@Id_Regalo;
GO
/****** Object:  StoredProcedure [dbo].[ModificarTicket]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ModificarTicket]
@Id_Ticket int,
@NumeroOperacion int,
@NombreDeudor varchar(300),
@IdentificacionDeudor varchar(10),
@Matricula varchar(10),
@Telefono int,
@DireccionDeudor varchar(255),
@EmailDeudor varchar(255),
@CuantiaDeuda decimal(18,2),
@Observaciones varchar(3800)
as
declare @FechaDePago datetime
set @FechaDePago = '2000-01-01 00:00:00'
Update TicketsPendientes
SET
NumeroOperacion = @NumeroOperacion,
NombreDeudor = @NombreDeudor,
IdentificacionDeudor = @IdentificacionDeudor,
Matricula = @Matricula,
Telefono = @Telefono,
DireccionDeudor = @DireccionDeudor,
EmailDeudor = @EmailDeudor,
CuantiaDeuda = @CuantiaDeuda,
FechaDePago = @FechaDePago,
Observaciones = @Observaciones
Where Id_Ticket = @Id_Ticket;
GO
/****** Object:  StoredProcedure [dbo].[ModificarTicketAPagado]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ModificarTicketAPagado]
@Id_ticket int,
@NumeroOperacion int,
@NombreDeudor varchar(300),
@IdentificacionDeudor varchar(10),
@Matricula varchar(10),
@Telefono int,
@DireccionDeudor varchar(255),
@EmailDeudor varchar(255),
@CuantiaDeuda decimal(18,2),
@FechaDePago Datetime,
@EstadoDeuda varchar(10),
@Observaciones varchar(3800)
as

Update TicketsPendientes
SET
NumeroOperacion = @NumeroOperacion,
NombreDeudor = @NombreDeudor,
IdentificacionDeudor = @IdentificacionDeudor,
Matricula = @Matricula,
Telefono = @Telefono,
DireccionDeudor = @DireccionDeudor,
EmailDeudor = @EmailDeudor,
CuantiaDeuda = @CuantiaDeuda,
FechaDePago = @FechaDePago,
EstadoDeuda = @EstadoDeuda,
Observaciones = @Observaciones
Where Id_Ticket = @Id_ticket;
GO
/****** Object:  StoredProcedure [dbo].[MostrarArticulo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[MostrarArticulo]
@Descripcion varchar(500)
as
Select * from Regalos where Descripcion LIKE '%'+@Descripcion+'%'
GO
/****** Object:  StoredProcedure [dbo].[MostrarArticuloCodigo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[MostrarArticuloCodigo]
@CodigoArticulo int
as
Select * from Regalos where CodigoArticulo = @CodigoArticulo
GO
/****** Object:  StoredProcedure [dbo].[MostrarPersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MostrarPersonal]
	@Desde int,
	@Hasta int
AS
Set Nocount On;
SELECT Id_Personal,Nombre,Identificacion,Pais,SueldoPorHora,Cargo,Id_cargo,Estado,Codigo from
	(SELECT p.Id_Personal, p.Nombre, p.Identificacion,p.Pais,p.SueldoPorHora,c.Cargo,p.Id_cargo, p.Estado,p.Codigo,
	Row_Number() Over(Order by p.Id_Personal) as 'num_fila'
	from Personal as p
	inner join Cargos as c on c.Id_cargo = p.Id_cargo) as Paginado
	where Paginado.num_fila >= @Desde and Paginado.num_fila <= @Hasta
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[mostrarTodasAsistencias]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[mostrarTodasAsistencias]
as
SELECT p.Nombre,p.Identificacion,a.Id_Personal, CONVERT(varchar, a.Fecha_Entrada, 120) AS Fecha_Entrada,
        CONVERT(varchar, a.Fecha_Salida, 120) AS Fecha_Salida,a.ObservacionesEntrada,a.ObservacionesSalida,a.Firma from Personal as p
INNER JOIN Asistencia as a on a.Id_Personal = p.Id_Personal;
GO
/****** Object:  StoredProcedure [dbo].[MostrarTodosArticulos]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[MostrarTodosArticulos]
as
Select * from Regalos
GO
/****** Object:  StoredProcedure [dbo].[MostrarTodosTickets]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[MostrarTodosTickets]
as
Select Id_Ticket,NumeroOperacion,NombreDeudor,Telefono,CuantiaDeuda,FechaDeDeuda,EstadoDeuda from TicketsPendientes
GO
/****** Object:  StoredProcedure [dbo].[MostrarTodosTicketsPagados]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[MostrarTodosTicketsPagados]
as
Select Id_Ticket,NumeroOperacion,NombreDeudor,Telefono,CuantiaDeuda,FechaDePago,EstadoDeuda from TicketsPendientes WHERE EstadoDeuda = 'PAGADO'
GO
/****** Object:  StoredProcedure [dbo].[MostrarTodosTicketsPendientes]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[MostrarTodosTicketsPendientes]
as
Select Id_Ticket,NumeroOperacion,NombreDeudor,Telefono,CuantiaDeuda,FechaDeDeuda,EstadoDeuda from TicketsPendientes WHERE EstadoDeuda = 'NO PAGADO'
GO
/****** Object:  StoredProcedure [dbo].[MostrarTodosTicketsPlus]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[MostrarTodosTicketsPlus]
as
Select * from TicketsPendientes Where EstadoDeuda = 'NO PAGADO'
GO
/****** Object:  StoredProcedure [dbo].[Update_Cargo]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Cargo]
	@Id_cargo int,
	@Cargo varchar(50),
	@SueldoPorHora numeric(18,2)
AS
if Exists(select Cargo from Cargos where Cargo = @Cargo and Id_cargo <> @Id_cargo)
raiserror('Ese cargo ya existe',16,1)
else
	Update Cargos set Cargo = @Cargo, SueldoPorHora = @SueldoPorHora where Id_Cargo = @Id_cargo
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[UpdateFecha_Hora]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateFecha_Hora] 
@Id_Personal int,
@ObservacionesSalida varchar(3800)
AS

if NOT EXISTS(select Fecha_Salida from Asistencia where Id_Personal = @Id_Personal and Fecha_Salida IS NULL)
RAISERROR('La identificaciÃ³n introducida no tiene ninguna entrada abierta. Abre primero una entrada para porder hacer una salida',16,2)
ELSE
Update Asistencia Set Fecha_Salida = GETDATE(), ObservacionesSalida = @ObservacionesSalida,
Horas = (Select Top 1 DATEDIFF
(HOUR,(select Fecha_Entrada from Asistencia where Id_Personal = @Id_Personal and Fecha_Salida IS NULL),GETDATE()) from Asistencia) -1
WHERE Id_Personal = @Id_Personal and Fecha_Salida IS NULL
GO
/****** Object:  StoredProcedure [dbo].[UpdatePersonal]    Script Date: 05/03/2024 19:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePersonal]
	@Id_Personal int,
    @Nombre VARCHAR(255),
    @Identificacion VARCHAR(255),
    @Pais VARCHAR(255),
    @Id_cargo INT ,
    @SueldoPorHora NUMERIC(18,2),
	@Estado VARCHAR(50)
	AS
	if Exists(select Identificacion from Personal where Identificacion = @Identificacion and Id_Personal <> @Id_personal)
	Raiserror('Esa Identificacion ya es existente en otro Usuario',16,1)
	else
	update Personal set Nombre = @Nombre,Identificacion = @Identificacion,Pais = @Pais,Id_cargo = @Id_cargo,
	SueldoPorHora = @SueldoPorHora,Estado = @Estado where Id_Personal = @Id_personal;
RETURN 0
GO
INSERT INTO Cargos Values ('Administrador',1);
GO
INSERT INTO Personal VALUES ('Administrador','0000000A','Espana',1,1,'ACTIVO','1',1);
GO
INSERT INTO Users Values (1,'Admin','E9CF3F0ECD469045446F50E517EB125A733482231AB0A93EE8C3492CDE823116');