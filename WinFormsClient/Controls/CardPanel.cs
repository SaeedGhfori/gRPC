using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace WinFormsClient.Controls;

/// <summary>پنل گرد با حاشیه — برای ساخت کارت‌های مدرن.</summary>
public class CardPanel : Panel
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CornerRadius { get; set; } = 14;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderColor { get; set; } = Color.FromArgb(226, 232, 240);
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int BorderWidth { get; set; } = 1;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color CardColor { get; set; } = Color.White;

    public CardPanel()
    {
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
        BackColor = Color.White;
    }

    public static GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
    {
        var r = Math.Max(1, Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2));
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, r * 2, r * 2, 180, 90);
        path.AddArc(bounds.Right - r * 2, bounds.Y, r * 2, r * 2, 270, 90);
        path.AddArc(bounds.Right - r * 2, bounds.Bottom - r * 2, r * 2, r * 2, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - r * 2, r * 2, r * 2, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? CardColor);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));

        using (var path = GetRoundedRect(rect, CornerRadius))
        using (var brush = new SolidBrush(CardColor))
        {
            g.FillPath(brush, path);
        }

        if (BorderWidth > 0)
        {
            using (var path = GetRoundedRect(rect, CornerRadius))
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                g.DrawPath(pen, path);
            }
        }

        base.OnPaint(e);
    }
}
