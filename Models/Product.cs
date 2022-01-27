using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pujcovna.Models
{
    public class Product
    {
        //vazební klíč 1:N na tabulku CategoryProduct
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vyplňte kód")]
        [StringLength(255, ErrorMessage = "Kód je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Kód produktu")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Vyplňte Url")]
        [StringLength(255, ErrorMessage = "Url je příliš dlouhá, max. 255 znaků")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Používejte jen malá písmena bez diakritiky nebo číslice")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Vyplňte titulek")]
        [StringLength(255, ErrorMessage = "Titulek je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Titulek produktu")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vyplňte značku")]
        [StringLength(255, ErrorMessage = "Krátký popis je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Značka")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Vyplňte krátký popis")]
        [StringLength(255, ErrorMessage = "Krátký popis je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Krátký popis")]
        public string ShortDescription { get; set; }

        [Display(Name = "Popis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vyplňte cenu bez DPH")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nesmí být záporná")]
        [Display(Name = "Cena bez DPH")]
        public decimal PriceWithoutDph { get; set; }

        [Required(ErrorMessage = "Vyplňte cenu s DPH")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nesmí být záporná")]
        [Display(Name = "Cena s DPH")]
        public decimal PriceWithDph { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hmotnost nesmí být záporná")]
        [Display(Name = "Hmotnost v kg")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Vyplňte počet kusů na skladě")]
        [Range(0, int.MaxValue, ErrorMessage = "Počet kusů na skladě nesmí být záporný")]
        [Display(Name = "Skladem")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Počet obrázků nesmí být záporný")]
        [Display(Name = "Obrázků produktu celkem")]
        public int? ImagesCount { get; set; }

        [Display(Name = "Skrýt")]
        public bool Hidden { get; set; }
                
        
        public Product()
        {
            ImagesCount = 0;
            Hidden = false;
            CategoryProducts = new List<CategoryProduct>();
        }
    }
}
