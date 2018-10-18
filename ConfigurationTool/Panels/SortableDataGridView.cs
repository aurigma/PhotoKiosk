// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System.Drawing;
using System.Windows.Forms;

namespace Aurigma.PhotoKiosk.ConfigurationTool
{
    public class SortableDataGridView : DataGridView
    {
        private Rectangle DragDropRectangle;
        private int DragDropSourceIndex;
        private int DragDropTargetIndex;
        private int DragDropCurrentIndex = -1;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (AllowDrop && HitTest(e.X, e.Y).ColumnIndex == -1 && HitTest(e.X, e.Y).RowIndex > -1 && Rows[HitTest(e.X, e.Y).RowIndex].Selected)
            {
                Size DragSize = SystemInformation.DragSize;
                DragDropRectangle = new Rectangle(new Point(e.X - (DragSize.Width / 2), e.Y - (DragSize.Height / 2)), DragSize);
                DragDropSourceIndex = HitTest(e.X, e.Y).RowIndex;
            }
            else
                DragDropRectangle = Rectangle.Empty;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (AllowDrop && (e.Button & MouseButtons.Left) == MouseButtons.Left && DragDropRectangle != Rectangle.Empty && !DragDropRectangle.Contains(e.X, e.Y))
                DoDragDrop(Rows[DragDropSourceIndex], DragDropEffects.Move);

            base.OnMouseMove(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (AllowDrop && e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow)
            {
                e.Effect = DragDropEffects.Move;
                int CurRow = HitTest(PointToClient(new Point(e.X, e.Y)).X, PointToClient(new Point(e.X, e.Y)).Y).RowIndex;
                if (DragDropCurrentIndex != CurRow)
                {
                    DragDropCurrentIndex = CurRow;
                    Invalidate();
                }
            }

            base.OnDragOver(e);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (AllowDrop && drgevent.Effect == DragDropEffects.Move && drgevent.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow)
            {
                Point ClientPoint = PointToClient(new Point(drgevent.X, drgevent.Y));

                DragDropTargetIndex = HitTest(ClientPoint.X, ClientPoint.Y).RowIndex;
                if (DragDropTargetIndex > -1 && DragDropCurrentIndex < RowCount - 1)
                {
                    DragDropCurrentIndex = -1;
                    DataGridViewRow SourceRow = drgevent.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    Rows.RemoveAt(DragDropSourceIndex);
                    Rows.Insert(DragDropTargetIndex, SourceRow);
                    Rows[DragDropTargetIndex].Selected = true;
                    CurrentCell = this[0, DragDropTargetIndex];
                }
            }

            base.OnDragDrop(drgevent);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (DragDropCurrentIndex > -1 && e.RowIndex == DragDropCurrentIndex && DragDropCurrentIndex < RowCount - 1)
            {
                Pen p = new Pen(Color.Red, 1);
                e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top - 1, e.CellBounds.Right, e.CellBounds.Top - 1);
            }

            base.OnCellPainting(e);
        }
    }
}