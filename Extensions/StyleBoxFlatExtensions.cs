using Godot;

namespace Godex.Extensions;

public static class StyleBoxFlatExtensions
{
    public static StyleBoxFlat Clone(this StyleBoxFlat styleBoxFlat)
    {
        var newBox = new StyleBoxFlat();
        newBox.BgColor = styleBoxFlat.BgColor;
        
        newBox.ContentMarginBottom = styleBoxFlat.ContentMarginBottom;
        newBox.ContentMarginTop = styleBoxFlat.ContentMarginTop;
        newBox.ContentMarginLeft = styleBoxFlat.ContentMarginLeft;
        newBox.ContentMarginRight = styleBoxFlat.ContentMarginRight;
        
        newBox.AntiAliasing = styleBoxFlat.AntiAliasing;
        newBox.AntiAliasingSize = styleBoxFlat.AntiAliasingSize;
        newBox.Skew = styleBoxFlat.Skew;
        
        newBox.BorderBlend = styleBoxFlat.BorderBlend;
        newBox.BorderColor = styleBoxFlat.BorderColor;
        newBox.BorderWidthTop = styleBoxFlat.BorderWidthTop;
        newBox.BorderWidthBottom = styleBoxFlat.BorderWidthBottom;
        newBox.BorderWidthLeft = styleBoxFlat.BorderWidthLeft;
        newBox.BorderWidthRight = styleBoxFlat.BorderWidthRight;
        
        newBox.ShadowColor = styleBoxFlat.ShadowColor;
        newBox.ShadowOffset = styleBoxFlat.ShadowOffset;
        newBox.ShadowSize = styleBoxFlat.ShadowSize;
        
        newBox.DrawCenter = styleBoxFlat.DrawCenter;
        newBox.ExpandMarginBottom = styleBoxFlat.ExpandMarginBottom;
        newBox.ExpandMarginTop = styleBoxFlat.ExpandMarginTop;
        newBox.ExpandMarginLeft = styleBoxFlat.ExpandMarginLeft;
        newBox.ExpandMarginRight = styleBoxFlat.ExpandMarginRight;

        return newBox;
    }
}