using CalcSustain.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcSustain.BLL.Interfaces
{
    /// <summary>
    /// Interface for cost calculating service
    /// </summary>
    public interface ICostService
    {
        /// <summary>
        /// Вычисление предполагаемого времени доставки из пункта А в пункт Б
        /// </summary>
        /// <param name="townFromId">Идентификатор пункта отправления</param>
        /// <param name="townToId">Идентификатор пункта назначения</param>
        /// <returns>Предполагаемое время доставки (в днях)</returns>
        int GetDelveryTime(int townFromId, int townToId);


        /// <summary>
        /// Подсчет стоимости доставки в зависимости от веса груза
        /// </summary>
        /// <param name="basic">Модель для вычисления базовой стоимости доставки груза определенного веса из пункта А в пункт Б</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        decimal PostBasicCost(BasicCostDTO basic);


        /// <summary>
        /// Подсчет стоимости доставки в зависимости от веса и объема груза
        /// </summary>
        /// <param name="volumeCostDTO">Модель для вычисления стоимости доставки груза определенного веса и объема из пункта А в пункт Б</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        decimal PostVolumeCost(VolumeCostDTO volumeCostDTO);


        /// <summary>
        /// Подсчет стоимости устройства обрешетки
        /// </summary>
        /// <param name="lathingCostDTO">Модель для вычисления стоимости обрешетки определенного объема, с определенным коэффициентом</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        decimal PostLathingCost(LathingCostDTO lathingCostDTO);


        /// <summary>
        /// Подсчет стоимости дополнительных услуг
        /// </summary>
        /// <param name="addServiceCostDTO">Модель для вычисления стоимости дополнительных услуг</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        decimal PostAddServiceCost(AddServiceCostDTO addServiceCostDTO);



        /// <summary>
        /// Подсчет стоимости: Забора от дверей отправителя / Доставки до дверей получателя
        /// </summary>
        /// <param name="pickUpCostDTO">Модель для: Забора от дверей отправителя / Доставки до дверей получателя</param>
        /// <returns>Стоимость в зависимости от входных параметров</returns>
        decimal PostPickUpCost(PickUpCostDTO pickUpCostDTO);

        /// <summary>
        /// Окончательный подсчет стоимости перевозки и доставки со всеми услугами
        /// </summary>
        /// <param name="volumeCost">Модель для вычисления стоимости доставки груза определенного веса и объема из пункта А в пункт Б</param>
        /// <param name="lathingCost">Модель для вычисления стоимости обрешетки определенного объема, с определенным коэффициентом</param>
        /// <param name="addServiceCost">Модель для вычисления стоимости дополнительных услуг</param>
        /// <param name="pickUpFromCost">Модель для: Забора от дверей отправителя</param>
        /// <param name="pickUpToCost">Модель для: Доставки до дверей получателя</param>
        /// <returns>Окончательная стоимость перевозки и доставки</returns>
        decimal PostTotalCost(VolumeCostDTO volumeCost, LathingCostDTO lathingCost, AddServiceCostDTO addServiceCost, PickUpCostDTO pickUpFromCost,PickUpCostDTO pickUpToCost);

    }
}
