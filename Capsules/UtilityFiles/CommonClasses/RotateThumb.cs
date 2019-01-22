using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Pictionary.Capsules.UtilityFiles
{
    class RotateThumb : Thumb
    {
        protected double _initialAngle;
        protected RotateTransform rotateRenderTransform;
        protected Vector _startVector;
        protected Point _middlePt;
        protected ContentControl _parentItem;
        protected Canvas mainCanvas;

        protected ContentControl ParentItem
        {
            get
            {
                if (_parentItem == null)
                {
                    _parentItem = this.DataContext as ContentControl;
                }
                return _parentItem;
            }
        }

        public RotateThumb()
        {
            this.Background = Brushes.YellowGreen;
            this.BorderBrush = Brushes.YellowGreen;
            DragDelta += new DragDeltaEventHandler(this.RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(this.RotateThumb_DragStarted);
            DragCompleted += new DragCompletedEventHandler(this.RotateThumb_DragCompleted);
            MouseEnter += RotateThumb_MouseEnter;
            MouseLeave += RotateThumb_MouseLeave;
        }

        void RotateThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void RotateThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            Thumb currentThumb = sender as Thumb;
            Cursor mousecursor = Cursors.Hand;
            Mouse.OverrideCursor = mousecursor;
        }

        protected void RotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            if (ParentItem is IRotateThumbOwner)
            {
                (ParentItem as IRotateThumbOwner).OnRotationComplete(0);
            }
        }

        protected virtual void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if ( (this.mainCanvas != null) && (this._parentItem != null) ) {
                Point pt1 = Mouse.GetPosition(this.mainCanvas);
                Vector angleVector = Point.Subtract(pt1, this._middlePt);
                RotateTransform _rotateTransform = this._parentItem.RenderTransform as RotateTransform;
                double ang = Vector.AngleBetween(this._startVector, angleVector);
                _rotateTransform.Angle = this._initialAngle + Math.Round(ang, 0);
                if (ParentItem is IRotateThumbOwner)
                {
                    (ParentItem as IRotateThumbOwner).WhileRotating(_rotateTransform.Angle);
                }
            }
        }

        protected void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this._parentItem = DataContext as ContentControl;
           
            if (this._parentItem != null)
            {
                this.mainCanvas = VisualTreeHelper.GetParent(this._parentItem) as Canvas;

                if (this.mainCanvas != null)
                {
                    this._middlePt = this._parentItem.TranslatePoint(
                        new Point(this._parentItem.Width * this._parentItem.RenderTransformOrigin.X,
                                  this._parentItem.Height * this._parentItem.RenderTransformOrigin.Y),
                                  this.mainCanvas);

                    Point pt1 = Mouse.GetPosition(this.mainCanvas);
                    this._startVector = Point.Subtract(pt1, this._middlePt);

                    this.rotateRenderTransform = this._parentItem.RenderTransform as RotateTransform;
                    if (this.rotateRenderTransform == null)
                    {
                        this._parentItem.RenderTransform = new RotateTransform(0);
                        this._initialAngle = 0;
                    }
                    else
                    {
                        this._initialAngle = this.rotateRenderTransform.Angle;
                    }
                   
                }                
             
            }
        }
    }
}
