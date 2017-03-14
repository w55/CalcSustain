using CalcSustain.WEB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;


//++  Calc service + Ninject 
using CalcSustain.BLL.Interfaces;
using CalcSustain.BLL.Infrastructure;
using CalcSustain.BLL.Services;
using Ninject;
using AutoMapper;
using CalcSustain.BLL.DTO;

namespace CalcSustain.WEB.Controllers
{
    public class ValuesController : ApiController
    {
        /* ------------------------------     DEBUG only helper function:     ShowAllHeaders  >>>  -----------------------------------  */		
        /// <summary>Получим все заголовки запроса, метод и запрашиваемый URL</summary>
        /// <returns>заголовки запроса, метод и запрашиваемый URL</returns>
        private string ShowAllHeaders()
        {
            Trace.WriteLine("- Request.Method = " + Request.Method + ", .RequestUri = " + Request.RequestUri + " -");
            bool isLocal = System.Web.HttpContext.Current.Request.IsLocal;
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            Trace.WriteLine("- UserHostAddress = " + userHostAddress + ", Request.IsLocal = " + isLocal + " -");

            // Получим все заголовки запроса:
            Trace.WriteLine("--- Request.Headers ---");
            string data = "";
            foreach (var o in Request.Headers)
            {
                string val = "";
                foreach (string s in o.Value)
                {
                    val += s + ";  ";
                }
                Trace.WriteLine(o.Key + " : " + val);
                data += o.Key + " : " + val;
            }
            return data;
        }
        /* ---------------------------------   <<<  DEBUG only helper function:     ShowAllHeaders   --------------------------------  */



        /* ------------------------------     Calc service + Ninject    -----------------------------------  */
        static ICostService CostService;

        static ValuesController()
        {
            // устанавливает использование CostService в качестве объекта ICostService
            //
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ICostService>().To<CostService>();
            CostService = ninjectKernel.Get<ICostService>();
        }
        /* ------------------------------     Calc service + Ninject     -----------------------------------  */


        //
        //-------------------    costService.GetDelveryTime()   -----------------------------------
        //
        //GET:  api/values/delivery/1/4
        //
        [Route("api/values/delivery/{townFromId:int}/{townToId:int}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetDelveryTime(int townFromId, int townToId)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            //----------   обработка частных случаев валидации  ----------------->>
            //

            // Для добавления дополнительной ошибки используется метод ModelState.AddModelError, 
            // первый параметр которого - ключ ошибки, а второй - сообщение об ошибке. 

            if (townFromId < 1)
                ModelState.AddModelError("townFromId", "Такого населенного пункта в базе данных не существует!");

            if (townToId < 1)
                ModelState.AddModelError("townToId", "Такого населенного пункта в базе данных не существует!");

            if (townFromId == townToId)
                ModelState.AddModelError("common", "Города отправления и назначения - совпадают");

            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.GetDelveryTime()
            //
            int days = 0;
            try
            {
                days = CostService.GetDelveryTime(townFromId, townToId);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//

            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.GetDelveryTime({0},{1})", townFromId, townToId);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, days.ToString());

            Trace.WriteLine(s_output + " = " + days);

            return Ok(days);
        }


        //
        //-----------    costService.PostBasicCost()   ------------------------
        //
        // POST: api/values/cost
        //
        [Route("api/values/cost")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostBasicCost(BasicCostWEB basic)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (basic == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (basic.TownFromId < 1)
                ModelState.AddModelError("TownFromId", "Такого населенного пункта в базе данных не существует!");

            if (basic.TownToId < 1)
                ModelState.AddModelError("TownToId", "Такого населенного пункта в базе данных не существует!");

            if (basic.TownFromId == basic.TownToId)
                ModelState.AddModelError("common", "Города отправления и назначения - совпадают");

            if (basic.Weight <= 0)
                ModelState.AddModelError("Weight", "Вес указан некорректно!");

            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostBasicCost()
            //
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<BasicCostWEB, BasicCostDTO>());
                var basicCostDTO = Mapper.Map<BasicCostWEB, BasicCostDTO>(basic);
                basic.Cost = CostService.PostBasicCost(basicCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostBasicCost({0},{1},{2})", basic.TownFromId, basic.TownToId, basic.Weight);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, basic.Cost.ToString());

            Trace.WriteLine(s_output + " = " + basic.Cost);

            return Ok(basic.Cost);
        }


        //
        //-----------    costService.PostVolumeCost()   ------------------------
        //
        // POST: api/values/volume
        //
        [Route("api/values/volume")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostVolumeCost(VolumeCostWEB volume)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (volume == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (volume.TownFromId < 1)
                ModelState.AddModelError("TownFromId", "Такого населенного пункта в базе данных не существует!");

            if (volume.TownToId < 1)
                ModelState.AddModelError("TownToId", "Такого населенного пункта в базе данных не существует!");

            if (volume.TownFromId == volume.TownToId)
                ModelState.AddModelError("common", "Города отправления и назначения - совпадают");

            if (volume.Weight <= 0)
                ModelState.AddModelError("Weight", "Вес указан некорректно!");

            if (volume.Volume < 0)
                ModelState.AddModelError("Volume", "Объем указан некорректно!");

            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostVolumeCost()
            //
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<VolumeCostWEB, VolumeCostDTO>());
                var volumeCostDTO = Mapper.Map<VolumeCostWEB, VolumeCostDTO>(volume);
                volume.Cost = CostService.PostVolumeCost(volumeCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostVolumeCost({0},{1},{2})", volume.TownFromId, volume.TownToId, volume.Weight);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, volume.Cost.ToString());

            Trace.WriteLine(s_output + " = " + volume.Cost);

            return Ok(volume.Cost);
        }



        //
        //-----------    costService.PostLathingCost()   ------------------------
        //
        // POST: api/values/lathing
        //
        [Route("api/values/lathing")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostLathingCost(LathingCostWEB lathing)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (lathing == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (lathing.Volume < 0)
                ModelState.AddModelError("Volume", "Объем указан некорректно!");

            if (lathing.Ratio < 1)
                ModelState.AddModelError("Ratio", "Коэффициент указан некорректно!");


            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostLathingCost()
            //
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<LathingCostWEB, LathingCostDTO>());
                var lathingCostDTO = Mapper.Map<LathingCostWEB, LathingCostDTO>(lathing);
                lathing.Cost = CostService.PostLathingCost(lathingCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostLathingCost({0},{1})", lathing.Volume, lathing.Ratio);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, lathing.Cost.ToString());

            Trace.WriteLine(s_output + " = " + lathing.Cost);

            return Ok(lathing.Cost);
        }




        //
        //-----------    costService.PostAddServiceCost()   ------------------------
        //
        // POST: api/values/service
        //
        [Route("api/values/service")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostAddServiceCost(AddServiceCostWEB serviceCost)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (serviceCost == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (serviceCost.DeclaredValue.HasValue && serviceCost.DeclaredValue.Value < 0)
                ModelState.AddModelError("Volume", "Объявленная ценность указана некорректно!");


            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostAddServiceCost()
            //
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<AddServiceCostWEB, AddServiceCostDTO>());
                var addServiceCostDTO = Mapper.Map<AddServiceCostWEB, AddServiceCostDTO>(serviceCost);
                serviceCost.Cost = CostService.PostAddServiceCost(addServiceCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostAddServiceCost({0},{1},{2},{3},{4},{5},{6})",
                serviceCost.Warmly, serviceCost.LocalStore, serviceCost.Brittle, serviceCost.ReCount, serviceCost.Packing, serviceCost.Insurance,
                serviceCost.DeclaredValue.HasValue ? serviceCost.DeclaredValue.Value.ToString() : "null");
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, serviceCost.Cost.ToString());

            Trace.WriteLine(s_output + " = " + serviceCost.Cost);

            return Ok(serviceCost.Cost);
        }


        //
        //-----------    costService.PostPickUpCost()   ------------------------
        //
        // POST: api/values/pickup
        //
        [Route("api/values/pickup")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostPickUpCost(PickUpCostWEB pickupCost)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (pickupCost == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (pickupCost.TownId.HasValue && pickupCost.TownId.Value <= 0)
                ModelState.AddModelError("TownId", "Указанного города нет в списке.");


            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostPickUpCost()
            //
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<PickUpCostWEB, PickUpCostDTO>());
                var pickUpCostDTO = Mapper.Map<PickUpCostWEB, PickUpCostDTO>(pickupCost);
                pickupCost.Cost = CostService.PostPickUpCost(pickUpCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostAddServiceCost({0},{1},{2},{3},{4},{5},{6},{7})",
                pickupCost.TownId ?? pickupCost.TownId.Value, pickupCost.NotExistedTown, pickupCost.Oversized, pickupCost.Warmly, pickupCost.AfterHours, pickupCost.FixedTime, pickupCost.Idling, pickupCost.Discharge);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, pickupCost.Cost.ToString());

            Trace.WriteLine(s_output + " = " + pickupCost.Cost);

            return Ok(pickupCost.Cost);
        }


        //
        //-----------    costService.PostTotalCost()   ------------------------
        //
        // POST: api/values/total
        //
        [Route("api/values/total")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult PostTotalCost(TotalCostWEB totalCostWEB)
        {
            // Получим все заголовки запроса:
            ShowAllHeaders();

            // отправка статусного кода 400
            if (totalCostWEB == null)
                return BadRequest();

            //----------   обработка частных случаев валидации  ----------------->>
            //
            if (totalCostWEB.volumeCost.TownFromId == totalCostWEB.volumeCost.TownToId)
                ModelState.AddModelError("TownToId", "Город отправления не может совпадать с городом получения");


            // Все ошибки валидаци сохраняются в объекте ModelState, который передается в метод BadRequest и, 
            // таким образом, отправляется клиенту вместе с ошибкой 400.
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // costService.PostTotalCost()
            //
            decimal totalCost = 0;
            try
            {
                Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<VolumeCostWEB, VolumeCostDTO>();
                        cfg.CreateMap<LathingCostWEB, LathingCostDTO>();
                        cfg.CreateMap<AddServiceCostWEB, AddServiceCostDTO>();
                        cfg.CreateMap<PickUpCostWEB, PickUpCostDTO>();
                    });

                var volumeCostDTO = Mapper.Map<VolumeCostWEB, VolumeCostDTO>(totalCostWEB.volumeCost);
                var lathingCostDTO = Mapper.Map<LathingCostWEB, LathingCostDTO>(totalCostWEB.lathingCost);
                var addServiceCostDTO = Mapper.Map<AddServiceCostWEB, AddServiceCostDTO>(totalCostWEB.addServiceCost);
                var pickUpFromCostDTO = Mapper.Map<PickUpCostWEB, PickUpCostDTO>(totalCostWEB.pickUpFromCost);
                var pickUpToCostDTO = Mapper.Map<PickUpCostWEB, PickUpCostDTO>(totalCostWEB.pickUpToCost);

                totalCost = CostService.PostTotalCost(volumeCostDTO, lathingCostDTO, addServiceCostDTO, pickUpFromCostDTO, pickUpToCostDTO);
            }
            catch (ValidationException x)
            {
                ModelState.AddModelError(x.Property, x.Message);
                return BadRequest(ModelState);
            }
            catch (Exception x)
            {
                ModelState.AddModelError("common", x.Message);
                return BadRequest(ModelState);
            }
            //
            //----------   обработка частных случаев валидации  -----------------//


            //++ Add output data to Headers collection
            string s_output = string.Format("x-costService.PostTotalCost({0},{1},{2},{3})",
                totalCostWEB.volumeCost.TownFromId, totalCostWEB.volumeCost.TownToId, totalCostWEB.volumeCost.Weight, totalCostWEB.volumeCost.Volume);
            System.Web.HttpContext.Current.Response.Headers.Add(s_output, totalCost.ToString());

            Trace.WriteLine(s_output + " = " + totalCost);

            return Ok(totalCost);
        }


    }
}
