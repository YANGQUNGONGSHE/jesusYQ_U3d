using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendationModel  {

    public string Id { get; set; }
    public string ContentType { get; set; }
    public string ContentId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string PictureUrl { get; set; }
    public int Position { get; set; }
    public int CreatedDate { get; set; }
    public Texture2D PicTexture2D { get; set; }
}
