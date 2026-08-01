using MudBlazor;

namespace LojaPedidos.Web.Theme;

public static class AppTheme
{
    public static MudTheme Default { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#0F3560",
            PrimaryContrastText = "#FFFFFF",
            PrimaryDarken = "#0A294B",
            PrimaryLighten = "#526F8E",
            Secondary = "#DF542A",
            SecondaryContrastText = "#FFFFFF",
            SecondaryDarken = "#B9401D",
            SecondaryLighten = "#F27A55",
            Tertiary = "#237A57",
            TertiaryContrastText = "#FFFFFF",
            Background = "#F5F7FA",
            Surface = "#FFFFFF",
            AppbarBackground = "#0F3560",
            AppbarText = "#FFFFFF",
            TextPrimary = "#1F2937",
            TextSecondary = "#526477",
            TextDisabled = "#8A98A8",
            LinesDefault = "#D8E0E8",
            TableLines = "#D8E0E8",
            Divider = "#D8E0E8",
            Success = "#237A57",
            Warning = "#B7791F",
            Error = "#B42318",
            Info = "#2563A6"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Montserrat", "Segoe UI", "Arial", "sans-serif"],
                FontSize = "15px",
                LineHeight = "1.5"
            },
            H1 = new H1Typography
            {
                FontFamily = ["Montserrat", "Segoe UI", "Arial", "sans-serif"],
                FontSize = "28px",
                FontWeight = "700",
                LineHeight = "1.2"
            },
            H2 = new H2Typography
            {
                FontFamily = ["Montserrat", "Segoe UI", "Arial", "sans-serif"],
                FontSize = "26px",
                FontWeight = "700",
                LineHeight = "1.25"
            },
            H4 = new H4Typography
            {
                FontFamily = ["Montserrat", "Segoe UI", "Arial", "sans-serif"],
                FontSize = "22px",
                FontWeight = "700",
                LineHeight = "1.3"
            },
            Button = new ButtonTypography
            {
                FontSize = "14px",
                FontWeight = "600",
                TextTransform = "none"
            }
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "12px",
            AppbarHeight = "68px"
        }
    };
}
