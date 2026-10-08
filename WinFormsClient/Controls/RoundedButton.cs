using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace WinFormsClient.Controls;

/// <summary>دکمه گرد تخت با افکت هاور/فشرده — برای ظاهر مدرن.</summary>
public class RoundedButton : Button
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int CornerRadius { get; set; } = 10;

    private bool _hover;
    private bool _pressed;

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = Color.Transparent;
        FlatAppearance.MouseDownBackColor = Color.Transparent;
        UseVisualStyleBackColor = false;
        Cursor = Cursors.Hand;
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
    }

    private static Color Shade(Color color, float factor)
    {
        // factor: 1 = بدون تغییر، کمتر از 1 = تیره‌تر
        return Color.FromArgb(
            color.A,
            Math.Clamp((int)(color.R * factor), 0, 255),
            Math.Clamp((int)(color.G * factor), 0, 255),
            Math.Clamp((int)(color.B * factor), 0, 255));
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _hover = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hover = false;
        _pressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _pressed = true;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _pressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var fill = BackColor;
        var fore = ForeColor;
        if (!Enabled)
        {
            fill = Color.FromArgb(203, 213, 225);
            fore = Color.FromArgb(100, 116, 139);
        }
        else if (_pressed)
        {
            fill = Shade(BackColor, 0.76f);
        }
        else if (_hover)
        {
            fill = Shade(BackColor, 0.88f);
        }

        var rect = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
        using (var path = CardPanel.GetRoundedRect(rect, CornerRadius))
        using (var brush = new SolidBrush(fill))
        {
            g.FillPath(brush, path);
        }

        TextRenderer.DrawText(
            g,
            Text,
            Font,
            rect,
            fore,
            TextFormatFlags.HorizontalCenter |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis);
    }
}
