# CalcSustain
<b>N-layer</b> ASP.NET <b>Web API 2</b> application using frameworks: <b>EntityFramewok</b>, <b>Ninject</b>, <b>Ninject.MVC5</b> and <b>Automapper</b>.
<br>
Authorization for this application uses <b>Oath2</b> specification based on <b>OWIN</b> middleware and <b>access tokens</b> produced by this middleware.
<hr>
Some key code snippets only
<br>
This is a 3-layer ASP.NET <b>Web API 2</b> application contains <b>DAL</b>, <b>BLL</b> and <b>WEB</b> layers with this schema:
<hr>
<img src="Screenshots/NLayer.jpg" alt="schema" width="300" />
<hr>

<br>
Here is current working schema for <b>OWIN</b>-based <b>access tokens</b> authorization:
<hr>
<img src="Screenshots/schema tokens access.jpg" alt="schema" width="700" />
<hr>

Take a look at layers code: 

<b>DAL</b> (Data Access Layer): 
<ul>
	<li>EF: <span data-href="CalcSustain.DAL/EF/OrderContext.cs">OrderContext.cs</span>,</li>
	<li>Entities: <span data-href="CalcSustain.DAL/Entities/Manager.cs">Manager.cs</span>,</li>
	<li>Interfaces: <span data-href="CalcSustain.DAL/Interfaces/IRepository.cs">IRepository.cs</span>,</li>
	<li>Repositories: <span data-href="CalcSustain.DAL/Repositories/OrderRepository.cs">OrderRepository.cs</span>,</li>
	<li>Repositories: <span data-href="CalcSustain.DAL/Repositories/ManagerRepository.cs">ManagerRepository.cs</span>,</li>
	<li>Repositories: <span data-href="CalcSustain.DAL/Repositories/EFUnitOfWork.cs">EFUnitOfWork.cs</span></li>
</ul>

<b>BLL</b> (Business Logic Layer): 
<ul>
	<li>DTO: <a href="CalcSustain.BLL/DTO/CalcModelsDTO.cs">CalcModelsDTO.cs</a>,</li>
	<li>Infrastructure: <a href="CalcSustain.BLL/Infrastructure/ValidationException.cs">ValidationException.cs</a>,</li>
	<li>Interfaces: <a href="CalcSustain.BLL/Interfaces/ICostService.cs">ICostService.cs</a>,</li>
	<li>Services: <a href="CalcSustain.BLL/Services/CostService.cs">CostService.cs</a></li>
</ul>

<b>WEB</b> (Presentation Layer): 
<ul>
	<li>App_Start: <a href="CalcSustain.WEB/App_Start/NinjectWebCommon.cs">NinjectWebCommon.cs</a>,</li>
	<li>App_Start: <a href="CalcSustain.WEB/App_Start/Startup.Auth.cs">Startup.Auth.cs</a>,</li>
	<li>App_Start: <a href="CalcSustain.WEB/App_Start/WebApiConfig.cs">WebApiConfig.cs</a>,</li>	
	
	<li>Controllers: <a href="CalcSustain.WEB/Controllers/AccountController.cs">AccountController.cs</a>,</li>
	<li>Controllers: <a href="CalcSustain.WEB/Controllers/ValuesController.cs">ValuesController.cs</a>,</li>	
	
	<li>Models: <a href="CalcSustain.WEB/Models/AccountDbInitializer.cs">AccountDbInitializer.cs</a>,</li>
	<li>Models: <a href="CalcSustain.WEB/Models/CalcModelsWEB.cs">CalcModelsWEB.cs</a>,</li>
	<li>Models: <a href="CalcSustain.WEB/Models/IdentityModels.cs">IdentityModels.cs</a>,</li>
	
	<li>Providers: <a href="CalcSustain.WEB/Models/ApplicationOAuthProvider.cs">ApplicationOAuthProvider.cs</a></li>
</ul>
<hr>

<p>Here are some screenshots:</p>
<p>
<b>HTML</b> page to interact with this <b>Web API2</b> application:
<hr>
<img width="600" src="Screenshots/calc_cost.jpg" alt="calc_cost.jpg" />
<hr>
