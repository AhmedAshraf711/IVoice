﻿
namespace IVoice.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        [AllowedExtension(FileSettings.AllowedExtensions),
          MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile ImageFile { get; set; }
    }
}
