using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CalcSustain.WEB.Models
{
    public class TownWEB
    {
        public int Id { get; set; }

        [Display(Name = "Название населенного пункта")]
        [Required(ErrorMessage = "Не введено название населенного пункта")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Название населенного пункта должно содержать на менее 1 и не более 100 символов")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Модель для вычисления времени доставки из пункта А в пункт Б
    /// </summary>
    public class DeliveryTimeWEB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан пункт отправления")]
        public int TownFromId { get; set; }

        [Required(ErrorMessage = "Не указан пункт назначения")]
        public int TownToId { get; set; }

        [Display(Name="Срок доставки (в сутках с момента сдачи груза)")]
        public int? DeliveryDays { get; set; }
    }

    /// <summary>
    /// Модель для вычисления базовой стоимости доставки груза определенного веса из пункта А в пункт Б
    /// </summary>
    public class BasicCostWEB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан пункт отправления")]
        public int TownFromId { get; set; }

        [Required(ErrorMessage = "Не указан пункт назначения")]
        public int TownToId { get; set; }

        [Required(ErrorMessage = "Не указан вес груза")]
        public decimal Weight { get; set; }

        [Display(Name = "Базовая стоимость доставки указанного груза")]
        public decimal? Cost { get; set; }
    }

    /// <summary>
    /// Модель для вычисления стоимости доставки груза определенного веса и объема из пункта А в пункт Б
    /// </summary>
    public class VolumeCostWEB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан пункт отправления")]
        public int TownFromId { get; set; }

        [Required(ErrorMessage = "Не указан пункт назначения")]
        public int TownToId { get; set; }

        [Required(ErrorMessage = "Не указан вес груза")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Не указан объем груза")]
        public decimal Volume { get; set; }

        [Display(Name = "Высота груза")]
        public decimal? Height { get; set; }

        [Display(Name = "Ширина груза")]
        public decimal? Width { get; set; }

        [Display(Name = "Глубина груза")]
        public decimal? Depth { get; set; }

        [Display(Name = "Базовая стоимость доставки указанного груза")]
        public decimal? Cost { get; set; }
    }

    /// <summary>
    /// Модель для вычисления стоимости обрешетки определенного объема, с определенным коэффициентом
    /// </summary>
    public class LathingCostWEB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан объем груза")]
        public decimal Volume { get; set; }

        [Required(ErrorMessage = "Не указан коэффициент")]
        public decimal Ratio { get; set; }

        [Display(Name = "Базовая стоимость доставки указанного груза")]
        public decimal? Cost { get; set; }
    }


    /// <summary>
    /// Модель для вычисления стоимости дополнительных услуг
    /// </summary>
    public class AddServiceCostWEB
    {
        public int Id { get; set; }

        [Display(Name = "Перевозка в тепле")]
        public bool Warmly { get; set; }
        
        [Display(Name = "Локальный склад")]
        public bool LocalStore { get; set; }
        
        [Display(Name = "Хрупкое отправление")]
        public bool Brittle { get; set; }
        
        [Display(Name = "Внутритарный пересчет")]
        public bool ReCount { get; set; }
        
        [Display(Name = "Укладка и опломбирование мест")]
        public bool Packing { get; set; }
        
        [Display(Name = "Страхование груза")]
        public bool Insurance { get; set; }

        [Display(Name = "Объявленная ценность")]
        public decimal? DeclaredValue { get; set; }

        [Display(Name = "Стоимость дополнительных услуг")]
        public decimal? Cost { get; set; }
    }

    /// <summary>
    /// Модель для: Забора от дверей отправителя / Доставки до дверей получателя
    /// </summary>
    public class PickUpCostWEB
    {
        public int Id { get; set; }

        [Display(Name = "Город отправления / назначения")]
        public int? TownId { get; set; }

        [Display(Name = "Указанного города нет в списке")]
        public bool NotExistedTown { get; set; }

        [Display(Name = "Негабаритный груз")]
        public bool Oversized { get; set; }

        [Display(Name = "Перевозка в тепле")]
        public bool Warmly { get; set; }

        [Display(Name = "Забор / доставка в нерабочее время")]
        public bool AfterHours { get; set; }

        [Display(Name = "Забор / доставка в фиксированное время")]
        public bool FixedTime { get; set; }

        [Display(Name = "Наличие холостого пробега")]
        public bool Idling { get; set; }

        [Display(Name = "Погрузка / Разгрузка силами экспедитора")]
        public bool Discharge { get; set; }

        [Display(Name = "Стоимость забора / доставки до дверей")]
        public decimal? Cost { get; set; }
    }

    /// <summary>
    /// Модель для вычисления общей суммы доставки
    /// </summary>
    public class TotalCostWEB
    {
        public VolumeCostWEB volumeCost {get; set;}
        public LathingCostWEB lathingCost {get; set;}
        public AddServiceCostWEB addServiceCost {get; set;}
        public PickUpCostWEB pickUpFromCost {get; set;}
        public PickUpCostWEB pickUpToCost { get; set; }
    }

}