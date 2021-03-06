﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoG.DAL.Entities
{
    public class AdvancedSearchModel
    {
        [MaxLength(256, ErrorMessage = "Title maximum length is 256 characters, make it shorter please")]
        public string Title { get; set; }

        [Display(Name = "Photoset date")]
        public DateTime? PhotosetDate { get; set; }

        [Display(Name = "Camera model")]
        [MaxLength(128, ErrorMessage = "Camera model name maximum length is 128 characters, make it shorter please")]
        public string CameraModel { get; set; }

        [Display(Name = "Lens Focal Length")]
        public double? LensFocalLength { get; set; }

        [MaxLength(128, ErrorMessage = "Diaphragm name maximum length is 128 characters, make it shorter please")]
        public string Diaphragm { get; set; }

        [Display(Name = "Shutter speed")]
        public double? ShutterSpeed { get; set; }
        public double? ISO { get; set; }

        [Display(Name = "Is flash in use")]
        public bool? FlashInUse { get; set; }
    }
}
