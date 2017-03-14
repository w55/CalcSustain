using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//++
using System.Diagnostics;
using CalcSustain.BLL.Interfaces;
using CalcSustain.BLL.Infrastructure;
using CalcSustain.BLL.DTO;

namespace CalcSustain.BLL.Services
{
    /// <summary>
    /// Implementation for  ICostService interface
    /// </summary>
    public class CostService : ICostService
    {
        /// <summary>
        /// Вычисление предполагаемого времени доставки из пункта А в пункт Б
        /// </summary>
        /// <param name="townFromId">Идентификатор пункта отправления</param>
        /// <param name="townToId">Идентификатор пункта назначения</param>
        /// <returns>Предполагаемое время доставки (в днях)</returns>
        public int GetDelveryTime(int townFromId, int townToId)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.GetDelveryTime({0},{1}) ---", townFromId, townToId);
            Trace.WriteLine(s_input);

            if (townFromId == townToId)
            {
                throw new ValidationException("Города отправления и назначения - совпадают", "townToId");
            }

            // cost calculating
            //
            int days = 0;

            /*--------------------------    Internal BLL logic -------------------------*/
            
            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.GetDelveryTime({0},{1}) = {2} ---", townFromId, townToId, days);
            Trace.WriteLine(s_output);

            return days;
        }


        /// <summary>
        /// Подсчет стоимости доставки в зависимости от веса груза
        /// </summary>
        /// <param name="basic">Модель для вычисления базовой стоимости доставки груза определенного веса из пункта А в пункт Б</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        public decimal PostBasicCost(BasicCostDTO basic)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostBasicCost({0},{1},{2}) ---", basic.TownFromId, basic.TownToId, basic.Weight);
            Trace.WriteLine(s_input);

            // валидация
            if (basic.TownFromId == basic.TownToId)
            {
                throw new ValidationException("Города отправления и назначения - совпадают", "TownToId");
            }

            if (basic.Weight <= 0 || basic.Weight >= 20000)
            {
                throw new ValidationException("Вес - должен быть более нуля и менее 20 тонн.", "Weight");
            }

            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostBasicCost({0},{1},{2}) = {3} ---", basic.TownFromId, basic.TownToId, basic.Weight, cost);
            Trace.WriteLine(s_output);

            return cost;
        }


        /// <summary>
        /// Подсчет стоимости доставки в зависимости от веса и объема груза
        /// </summary>
        /// <param name="volumeCostDTO">Модель для вычисления стоимости доставки груза определенного веса и объема из пункта А в пункт Б</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        public decimal PostVolumeCost(VolumeCostDTO volumeCostDTO)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostVolumeCost({0},{1},{2},{3}) ---", volumeCostDTO.TownFromId, volumeCostDTO.TownToId, volumeCostDTO.Weight, volumeCostDTO.Volume);
            Trace.WriteLine(s_input);

            // валидация
            if (volumeCostDTO.TownFromId == volumeCostDTO.TownToId)
            {
                throw new ValidationException("Города отправления и назначения - совпадают", "TownToId");
            }

            if (volumeCostDTO.Weight <= 0 || volumeCostDTO.Weight >= 20000)
            {
                throw new ValidationException("Вес - должен быть более нуля и менее 20 тонн.", "Weight");
            }

            if (volumeCostDTO.Volume < 0 || volumeCostDTO.Volume >= 20)
            {
                throw new ValidationException("Объем - должен быть не менее нуля и не более 20 куб.м.", "Volume");
            }

            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostBasicCost() = {0} ---", cost);
            Trace.WriteLine(s_output);

            return cost;
        }

        /// <summary>
        /// Подсчет стоимости устройства обрешетки
        /// </summary>
        /// <param name="lathingCostDTO">Модель для вычисления стоимости обрешетки определенного объема, с определенным коэффициентом</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        public decimal PostLathingCost(LathingCostDTO lathingCostDTO)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostLathingCost({0},{1}) ---", lathingCostDTO.Volume, lathingCostDTO.Ratio);
            Trace.WriteLine(s_input);

            // валидация
            if (lathingCostDTO.Volume < 0 || lathingCostDTO.Volume >= 20)
            {
                throw new ValidationException("Объем - должен быть не менее нуля и не более 20 куб.м.", "Volume");
            }
            if (lathingCostDTO.Ratio < 1 || lathingCostDTO.Ratio > 2)
            {
                throw new ValidationException("Коэффициент - должен быть в пределах от 1 до 2-х.", "Ratio");
            }

            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/
            
            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostLathingCost() = {0} ---", cost);
            Trace.WriteLine(s_output);

            return cost;
        }


        /// <summary>
        /// Подсчет стоимости дополнительных услуг
        /// </summary>
        /// <param name="addServiceCostDTO">Модель для вычисления стоимости дополнительных услуг</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        public decimal PostAddServiceCost(AddServiceCostDTO addServiceCostDTO)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostAddServiceCost({0},{1},{2},{3},{4},{5}) ---",
                addServiceCostDTO.Warmly, addServiceCostDTO.LocalStore, addServiceCostDTO.Brittle, addServiceCostDTO.ReCount, addServiceCostDTO.Packing, addServiceCostDTO.Insurance);
            Trace.WriteLine(s_input);

            // валидация
            if (addServiceCostDTO.DeclaredValue.HasValue && addServiceCostDTO.DeclaredValue.Value <= 0)
            {
                throw new ValidationException("Объявленная ценность - должна быть не менее нуля.", "DeclaredValue");
            }

            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostAddServiceCost() = {0} ---", cost);
            Trace.WriteLine(s_output);

            return cost;
        }


        /// <summary>
        /// Подсчет стоимости: Забора от дверей отправителя / Доставки до дверей получателя
        /// </summary>
        /// <param name="pickUpCostDTO">Модель для: Забора от дверей отправителя / Доставки до дверей получателя</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        public decimal PostPickUpCost(PickUpCostDTO pickUpCostDTO)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostPickUpCost({0},{1},{2},{3},{4},{5},{6},{7}) ---",
                pickUpCostDTO.TownId ?? pickUpCostDTO.TownId.Value, pickUpCostDTO.NotExistedTown, pickUpCostDTO.Oversized, pickUpCostDTO.Warmly, pickUpCostDTO.AfterHours, pickUpCostDTO.FixedTime, pickUpCostDTO.Idling, pickUpCostDTO.Discharge);
            Trace.WriteLine(s_input);

            // валидация
            if (pickUpCostDTO.TownId.HasValue && pickUpCostDTO.TownId.Value <= 0)
            {
                throw new ValidationException("Указанного города нет в списке", "TownId");
            }

            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostPickUpCost() = {0} ---", cost);
            Trace.WriteLine(s_output);

            return cost;
        }

        /// <summary>
        /// Окончательный подсчет стоимости перевозки и доставки со всеми услугами
        /// </summary>
        /// <param name="volumeCost">Модель для вычисления стоимости доставки груза определенного веса и объема из пункта А в пункт Б</param>
        /// <param name="lathingCost">Модель для вычисления стоимости обрешетки определенного объема, с определенным коэффициентом</param>
        /// <param name="addServiceCost">Модель для вычисления стоимости дополнительных услуг</param>
        /// <param name="pickUpFromCost">Модель для: Забора от дверей отправителя</param>
        /// <param name="pickUpToCost">Модель для: Доставки до дверей получателя</param>
        /// <returns>Окончательная стоимость перевозки и доставки</returns>
        public decimal PostTotalCost(VolumeCostDTO volumeCost, LathingCostDTO lathingCost, AddServiceCostDTO addServiceCost, PickUpCostDTO pickUpFromCost, PickUpCostDTO pickUpToCost)
        {
            // input data tracer
            //
            string s_input = string.Format("--- CostService.PostTotalCost({0},{1},{2},{3}) ---",
                volumeCost.TownFromId, volumeCost.TownToId, volumeCost.Weight, volumeCost.Volume);
            Trace.WriteLine(s_input);

            // валидация
            if (volumeCost.TownFromId == volumeCost.TownToId)
            {
                throw new ValidationException("Город отправления не может совпадать с городом получения", "TownToId");
            }
            
            // cost calculating
            //
            decimal cost = 0;

            /*--------------------------    Internal BLL logic -------------------------*/

            /*--------------------------    Internal BLL logic -------------------------*/

            // ouput data tracer
            //
            string s_output = string.Format("--- CostService.PostTotalCost() = {0} ---", cost);
            Trace.WriteLine(s_output);

            return cost;
        }
    }
}
