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
    public class ResizeThumb : Thumb
    {
        #region Variables

        protected ContentControl _parentItem;
        private ThumbPosition _clsCurrentThumbPosition;
        private Cursor _clsCurrentMouseCursor;
        private Point _rendertransformOrigin;
        private double _dblGlobalAngle, _dblRadianangle;

        #endregion

        #region Constructor

        public ResizeThumb()
        {
            UnregisterEvent();
            this.Background = Brushes.YellowGreen;
            this.BorderBrush = Brushes.YellowGreen;
            this.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
            this.DragCompleted += new DragCompletedEventHandler(ResizeThumb_DragCompleted);
            this.DragStarted += new DragStartedEventHandler(ResizeThumb_DragStarted);
            this.MouseEnter += new MouseEventHandler(ResizeThumb_MouseEnter);
            this.MouseLeave += new MouseEventHandler(ResizeThumb_MouseLeave);
            _rendertransformOrigin = new Point();

        }

        #endregion

        #region Properties

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

        #endregion

        #region Mouse Events

        void ResizeThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ResizeThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            Thumb currentThumb = sender as Thumb;
            _dblGlobalAngle = (ParentItem as IResizeThumbOwner).ImageAngle;
            double _thumbInitialAngle = _AngleForCurrentThumb(SetCurrentThumb(currentThumb.Name), _dblGlobalAngle);
            double _mainangle = _GetRelativeAngle(_thumbInitialAngle);
            Cursor mouseCursor = _GetCursorForSpecifiedAngle(_mainangle);
            Mouse.OverrideCursor = mouseCursor;
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (ParentItem != null)
            {
                _rendertransformOrigin = ParentItem.RenderTransformOrigin;

                RotateTransform _rotateTransform = ParentItem.RenderTransform as RotateTransform;
                if (_rotateTransform != null)
                {
                    _dblGlobalAngle = _rotateTransform.Angle;
                    _dblRadianangle = _rotateTransform.Angle * Math.PI / 180.0;
                }
                else
                {
                    _dblRadianangle = 0.0d;
                    _dblGlobalAngle = 0.0;
                }
            }
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (ParentItem is IResizeThumbOwner)
            {
                (ParentItem as IResizeThumbOwner).OnResizeComplete(new Point(Canvas.GetLeft(ParentItem), Canvas.GetTop(ParentItem)), new Point(Canvas.GetLeft(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlWidth, Canvas.GetTop(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlHeight));
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (null != this.DataContext)
            {
                ParentItem.Focus();
                Size _OldSize = new Size(ParentItem.ActualWidth, ParentItem.ActualHeight);
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    _DoDragDeltaUsingAngleForCtrlCase(_clsCurrentThumbPosition, e.HorizontalChange, e.VerticalChange, ParentItem);
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    if (_dblGlobalAngle != 0)
                    {
                        _DoDragDeltaUsingAngle(e.HorizontalChange, e.VerticalChange, ParentItem);
                    }
                    else
                    {
                        _DoDragDeltaUsingAngleForShiftCase(_clsCurrentThumbPosition, e.HorizontalChange, e.VerticalChange, ParentItem);
                    }
                }
                else if ((Keyboard.IsKeyDown(Key.LeftShift)) && (Keyboard.IsKeyDown(Key.LeftCtrl)) || (Keyboard.IsKeyDown(Key.RightShift)) && (Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    _DoDragDeltaUsingAngleForCtrlCase(_clsCurrentThumbPosition, e.HorizontalChange, e.VerticalChange, ParentItem);
                }
                else
                    _DoDragDeltaUsingAngle(e.HorizontalChange, e.VerticalChange, ParentItem);
                if (ParentItem is IResizeThumbOwner)
                {
                    (ParentItem as IResizeThumbOwner).WhileResizing(new Point(Canvas.GetLeft(ParentItem), Canvas.GetTop(ParentItem)), new Point(Canvas.GetLeft(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlWidth, Canvas.GetTop(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlHeight));
                }
                e.Handled = true;


            }


        }

        #endregion

        #region Maintenance Methods

        protected Cursor _GetCursorForSpecifiedAngle(double dblAngle)
        {
            Cursor clsMouseCursor = null;

            if ((dblAngle > -22.5 && dblAngle <= 22.5) || (dblAngle > 337.5 && dblAngle <= 360) || (dblAngle < -337.5 && dblAngle >= -360))
            {
                clsMouseCursor = Cursors.SizeNS;
            }
            else if ((dblAngle > 22.5 && dblAngle <= 67.5) || (dblAngle < -292.5 && dblAngle >= -337.5))
            {
                clsMouseCursor = Cursors.SizeNESW;
            }
            else if ((dblAngle > 67.5 && dblAngle <= 112.5) || (dblAngle < -247.5 && dblAngle >= -292.5))
            {
                clsMouseCursor = Cursors.SizeWE;
            }
            else if ((dblAngle > 112.5 && dblAngle <= 157.5) || (dblAngle < -202.5 && dblAngle >= -247.5))
            {
                clsMouseCursor = Cursors.SizeNWSE;
            }
            else if ((dblAngle > 157.5 && dblAngle <= 202.5) || (dblAngle < -157.5 && dblAngle >= -202.5))
            {
                clsMouseCursor = Cursors.SizeNS;
            }
            else if ((dblAngle > 202.5 && dblAngle <= 247.5) || (dblAngle < -112.5 && dblAngle >= -157.5))
            {
                clsMouseCursor = Cursors.SizeNESW;
            }
            else if ((dblAngle > 247.5 && dblAngle <= 292.5) || (dblAngle < -67.5 && dblAngle >= -112.5))
            {
                clsMouseCursor = Cursors.SizeWE;
            }
            else if ((dblAngle > 292.5 && dblAngle <= 337.5) || (dblAngle < -22.5 && dblAngle >= -67.5))
            {
                clsMouseCursor = Cursors.SizeNWSE;
            }
            return clsMouseCursor;
        }

        protected double _AngleForCurrentThumb(ThumbPosition _emThumbDragged, double dblAngle)
        {
            double _dblthumbAngle = 0;
            if (_emThumbDragged == ThumbPosition.TopCenter)
            {
                _dblthumbAngle = 0 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.TopRight)
            {
                _dblthumbAngle = 45 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.RightCenter)
            {
                _dblthumbAngle = 90 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.BottomRight)
            {
                _dblthumbAngle = 135 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.BottomCenter)
            {
                _dblthumbAngle = 180 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.BottomLeft)
            {
                _dblthumbAngle = 225 + dblAngle;
            }
            else if (_emThumbDragged == ThumbPosition.LeftCenter)
            {
                _dblthumbAngle = 270 + dblAngle;
            }
            if (_emThumbDragged == ThumbPosition.TopLeft)
            {
                _dblthumbAngle = 315 + dblAngle;
            }
            return _dblthumbAngle;
        }

        protected double _GetRelativeAngle(double dblAngle)
        {
            double _doubleThumbangle = dblAngle;

            if (dblAngle > 360)
            {
                _doubleThumbangle = dblAngle - 360;
            }
            return _doubleThumbangle;
        }

        private ThumbPosition SetCurrentThumb(string _strThumbPos)
        {
            switch (_strThumbPos)
            {
                case "ThumbTopLeft":
                    _clsCurrentThumbPosition = ThumbPosition.TopLeft;
                    break;
                case "ThumbTopRight":
                    _clsCurrentThumbPosition = ThumbPosition.TopRight;
                    break;
                case "ThumbBottomLeft":
                    _clsCurrentThumbPosition = ThumbPosition.BottomLeft;
                    break;
                case "ThumbBottomRight":
                    _clsCurrentThumbPosition = ThumbPosition.BottomRight;
                    break;
                case "ThumbTopCenter":
                    _clsCurrentThumbPosition = ThumbPosition.TopCenter;
                    break;
                case "ThumbBottomCenter":
                    _clsCurrentThumbPosition = ThumbPosition.BottomCenter;
                    break;
                case "ThumbLeftCenter":
                    _clsCurrentThumbPosition = ThumbPosition.LeftCenter;
                    break;
                case "ThumbRightCenter":
                    _clsCurrentThumbPosition = ThumbPosition.RightCenter;
                    break;
                default:
                    _clsCurrentThumbPosition = ThumbPosition.None;
                    break;
            }
            return _clsCurrentThumbPosition;
        }

        private Cursor GetMouseCursor(string _strThumbPos)
        {
            switch (_strThumbPos)
            {
                case "ThumbTopLeft":
                    _clsCurrentThumbPosition = ThumbPosition.TopLeft;
                    return _clsCurrentMouseCursor = Cursors.SizeNWSE;
                case "ThumbTopRight":
                    _clsCurrentThumbPosition = ThumbPosition.TopRight;
                    return _clsCurrentMouseCursor = Cursors.SizeNESW;
                case "ThumbBottomLeft":
                    _clsCurrentThumbPosition = ThumbPosition.BottomLeft;
                    return _clsCurrentMouseCursor = Cursors.SizeNESW;
                case "ThumbBottomRight":
                    _clsCurrentThumbPosition = ThumbPosition.BottomRight;
                    return _clsCurrentMouseCursor = Cursors.SizeNWSE;
                case "ThumbTopCenter":
                    _clsCurrentThumbPosition = ThumbPosition.TopCenter;
                    return _clsCurrentMouseCursor = Cursors.SizeNS;
                case "ThumbBottomCenter":
                    _clsCurrentThumbPosition = ThumbPosition.BottomCenter;
                    return _clsCurrentMouseCursor = Cursors.SizeNS;
                case "ThumbLeftCenter":
                    _clsCurrentThumbPosition = ThumbPosition.LeftCenter;
                    return _clsCurrentMouseCursor = Cursors.SizeWE;
                case "ThumbRightCenter":
                    _clsCurrentThumbPosition = ThumbPosition.RightCenter;
                    return _clsCurrentMouseCursor = Cursors.SizeWE;
                default:
                    _clsCurrentThumbPosition = ThumbPosition.None;
                    return _clsCurrentMouseCursor = Cursors.SizeWE;
            }
        }

        protected void _DoDragDeltaUsingAngleForShiftCase(ThumbPosition _emThumbDragged, double _dblHorizontalChange, double _dblVerticalChange, Control desControl)
        {
            ContentControl _contentctrlObj = desControl as ContentControl;
            double _dblPrevLeft = Canvas.GetLeft(_contentctrlObj);
            double _dblPrevTop = Canvas.GetTop(_contentctrlObj);
            double _dblPrevHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight;
            double _dblPrevWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth;

            double allowedHorizontalChange = _dblHorizontalChange / 12;
            double allowedVerticalChange = _dblVerticalChange / 12;

            double aspectRatio = _dblPrevHeight / _dblPrevWidth;

            switch (_emThumbDragged)
            {
                case ThumbPosition.RightCenter:
                    _DoDragDeltaUsingAngle(_dblHorizontalChange, _dblVerticalChange, desControl);
                    break;
                case ThumbPosition.LeftCenter:
                    _DoDragDeltaUsingAngle(_dblHorizontalChange, _dblVerticalChange, desControl);
                    break;
                case ThumbPosition.TopCenter:
                    _DoDragDeltaUsingAngle(_dblHorizontalChange, _dblVerticalChange, desControl);
                    break;
                case ThumbPosition.BottomCenter:
                    _DoDragDeltaUsingAngle(_dblHorizontalChange, _dblVerticalChange, desControl);
                    break;
                case ThumbPosition.BottomRight:
                    if (_dblVerticalChange > 0 || _dblHorizontalChange > 0)
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;
                        if (_dblVerticalChange > _dblHorizontalChange)
                        {
                            maximumDelta = _dblVerticalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = _dblHorizontalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double newHeight = (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlHeight;
                        double newWidth = (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlWidth;

                        if ((newHeight >= ParentItem.MinHeight) && (newWidth >= ParentItem.MinWidth))
                        {
                            (ParentItem as IResizeThumbOwner).ImageControlHeight = newHeight;
                            (ParentItem as IResizeThumbOwner).ImageControlWidth = newWidth;
                        }
                        else
                        {
                            SetAspectRatio(aspectRatio, newHeight, newWidth);
                        }

                    }
                    else
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;
                        if (_dblVerticalChange > _dblHorizontalChange)
                        {
                            maximumDelta = _dblVerticalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = _dblHorizontalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double _newHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight + (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100);
                        double _newWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth + (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100);
                        if ((_newHeight >= ParentItem.MinHeight) && (_newWidth >= ParentItem.MinWidth))
                        {
                            (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                            (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                        }
                        else
                        {
                            SetAspectRatio(aspectRatio, _newHeight, _newWidth);
                        }
                    }

                    break;

                case ThumbPosition.TopLeft:

                    if (_dblVerticalChange > 0 || _dblHorizontalChange > 0)
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;
                        if (_dblVerticalChange < _dblHorizontalChange)
                        {
                            maximumDelta = _dblVerticalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = _dblHorizontalChange;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double _newHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight - (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100);
                        double _newWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth - (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100);

                        if ((_newHeight >= ParentItem.MinHeight) && (_newWidth >= ParentItem.MinWidth))
                        {
                            Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - _newHeight);
                            Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - _newWidth);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                            (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                        }
                        else
                        {
                            SetAspectRatio(aspectRatio, _newHeight, _newWidth);
                            Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - (ParentItem as IResizeThumbOwner).ImageControlHeight);
                            Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - (ParentItem as IResizeThumbOwner).ImageControlWidth);
                        }
                    }
                    else
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;

                        double absoluteVerticalDelta = Math.Abs(_dblVerticalChange);
                        double absoluteHorizontalDelta = Math.Abs(_dblHorizontalChange);

                        if (_dblVerticalChange < 0 && _dblHorizontalChange < 0)
                        {
                            if (absoluteVerticalDelta > absoluteHorizontalDelta)
                            {
                                maximumDelta = absoluteVerticalDelta;
                                deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                            }
                            else
                            {
                                maximumDelta = absoluteHorizontalDelta;
                                deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                            }
                        }
                        else if (_dblHorizontalChange > 0 && _dblVerticalChange < 0)
                        {
                            maximumDelta = absoluteVerticalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = absoluteHorizontalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double _newHeight = (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlHeight;
                        double _newWidth = (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlWidth;

                        Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - _newHeight);
                        Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - _newWidth);

                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;

                    }

                    break;
                case ThumbPosition.TopRight:

                    if (_dblVerticalChange >= 0 && _dblHorizontalChange <= 0)
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;

                        double absoluteVerticalDelta = Math.Abs(_dblVerticalChange);
                        double absoluteHorizontalDelta = Math.Abs(_dblHorizontalChange);

                        if (absoluteVerticalDelta <= absoluteHorizontalDelta)
                        {
                            maximumDelta = absoluteVerticalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = absoluteHorizontalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double _newHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight - (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100);
                        double _newWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth - (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100);
                        if ((_newHeight >= ParentItem.MinHeight) && (_newWidth >= ParentItem.MinWidth))
                        {
                            Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - _newHeight);
                            Canvas.SetRight(ParentItem, Canvas.GetLeft(ParentItem) + _newWidth);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                            (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                        }
                        else
                        {
                            SetAspectRatio(aspectRatio, _newHeight, _newWidth);
                            Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - (ParentItem as IResizeThumbOwner).ImageControlHeight);
                            Canvas.SetRight(ParentItem, Canvas.GetLeft(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlWidth);
                        }
                    }
                    else
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;

                        double absoluteVerticalDelta = Math.Abs(_dblVerticalChange);
                        double absoluteHorizontalDelta = Math.Abs(_dblHorizontalChange);

                        if ((_dblHorizontalChange > 0 && _dblVerticalChange < _dblHorizontalChange) || _dblVerticalChange > 0)
                        {
                            maximumDelta = absoluteHorizontalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }
                        else
                        {
                            maximumDelta = absoluteVerticalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }

                        double _newHeight = (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlHeight;
                        double _newWidth = (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlWidth;
                        double _top = Canvas.GetTop(ParentItem);
                        double _bottom = Canvas.GetBottom(ParentItem);
                        Canvas.SetTop(ParentItem, Canvas.GetBottom(ParentItem) - _newHeight);
                        Canvas.SetRight(ParentItem, Canvas.GetLeft(ParentItem) + _newWidth);
                        double _newtop = Canvas.GetTop(ParentItem);
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                    }
                    break;
                case ThumbPosition.BottomLeft:
                    if (_dblHorizontalChange >= 0 && _dblVerticalChange <= 0)
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;

                        double absoluteVerticalDelta = Math.Abs(_dblVerticalChange);
                        double absoluteHorizontalDelta = Math.Abs(_dblHorizontalChange);

                        if (absoluteVerticalDelta <= absoluteHorizontalDelta)
                        {
                            maximumDelta = absoluteVerticalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else
                        {
                            maximumDelta = absoluteHorizontalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }

                        double _newHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight - (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100);
                        double _newWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth - (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100);
                        if ((_newHeight >= ParentItem.MinHeight) && (_newWidth >= ParentItem.MinWidth))
                        {
                            Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - _newWidth);
                            Canvas.SetBottom(ParentItem, Canvas.GetTop(ParentItem) + _newHeight);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                            (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                        }
                        else
                        {
                            SetAspectRatio(aspectRatio, _newHeight, _newWidth);
                            Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - (ParentItem as IResizeThumbOwner).ImageControlWidth);
                            Canvas.SetBottom(ParentItem, Canvas.GetTop(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlHeight);
                        }
                    }
                    else
                    {
                        double maximumDelta = 0;
                        double deltaPercentage = 0;

                        double absoluteVerticalDelta = Math.Abs(_dblVerticalChange);
                        double absoluteHorizontalDelta = Math.Abs(_dblHorizontalChange);

                        if (_dblVerticalChange > 0 && _dblHorizontalChange > 0)
                        {
                            maximumDelta = absoluteVerticalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                        }
                        else if (_dblVerticalChange < 0 && _dblHorizontalChange < 0)
                        {
                            maximumDelta = absoluteHorizontalDelta;
                            deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                        }
                        else
                        {
                            if (absoluteVerticalDelta > absoluteHorizontalDelta)
                            {
                                maximumDelta = absoluteVerticalDelta;
                                deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlHeight) * 100;
                            }
                            else
                            {
                                maximumDelta = absoluteHorizontalDelta;
                                deltaPercentage = (maximumDelta / (ParentItem as IResizeThumbOwner).ImageControlWidth) * 100;
                            }
                        }


                        double _newHeight = (((ParentItem as IResizeThumbOwner).ImageControlHeight * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlHeight;
                        double _newWidth = (((ParentItem as IResizeThumbOwner).ImageControlWidth * deltaPercentage) / 100) + (ParentItem as IResizeThumbOwner).ImageControlWidth;
                        Canvas.SetLeft(ParentItem, Canvas.GetRight(ParentItem) - _newWidth);
                        Canvas.SetBottom(ParentItem, Canvas.GetTop(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlHeight);
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _newHeight;
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _newWidth;
                    }
                    break;
            }
            if (CheckBoundaryConditions() == false)
            {
                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
            }
        }

        protected void SetAspectRatio(double aspectRatio, double newHeight, double newWidth)
        {
            if (newHeight < ParentItem.MinHeight && newWidth >= ParentItem.MinWidth)
            {
                (ParentItem as IResizeThumbOwner).ImageControlHeight = ParentItem.MinHeight;
                (ParentItem as IResizeThumbOwner).ImageControlWidth = (ParentItem as IResizeThumbOwner).ImageControlHeight / aspectRatio;
            }
            if (newWidth < ParentItem.MinWidth && newHeight >= ParentItem.MinHeight)
            {
                (ParentItem as IResizeThumbOwner).ImageControlWidth = ParentItem.MinWidth;
                (ParentItem as IResizeThumbOwner).ImageControlHeight = (ParentItem as IResizeThumbOwner).ImageControlWidth * aspectRatio;
            }
        }

        protected void _DoDragDeltaUsingAngleForCtrlCase(ThumbPosition _emThumbDragged, double _dblHorizontalChange, double _dblVerticalChange, Control desControl)
        {
            double _dbldeltaVertical, _dbldeltaHorizontal = 0;
            ContentControl _contentctrlObj = desControl as ContentControl;
            double _dblPrevLeft = Canvas.GetLeft(_contentctrlObj);
            double _dblPrevTop = Canvas.GetTop(_contentctrlObj);
            double _dblPrevHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight;
            double _dblPrevWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth;

            double allowedHorizontalChange = _dblHorizontalChange / 12;
            double allowedVerticalChange = _dblVerticalChange / 12;

            switch (_emThumbDragged)
            {
                case ThumbPosition.BottomCenter:
                    if (_dblVerticalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange > 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }


                    break;
                case ThumbPosition.BottomLeft:
                    if (_dblVerticalChange < 0 && _dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight && (ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal >= ParentItem.MinWidth)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                                Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) - _dbldeltaHorizontal);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - Math.Abs(_dbldeltaHorizontal * 2));
                            }
                        }
                    }

                    else if (_dblVerticalChange > 0 && _dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(-_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(-_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) + _dbldeltaVertical);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += Math.Abs(_dbldeltaHorizontal * 2);
                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    if (_dblHorizontalChange < 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);

                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }
                    else if (_dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (_dblVerticalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange > 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    break;
                case ThumbPosition.BottomRight:

                    if (_dblVerticalChange < 0 && _dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange) && _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if (((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight) && ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal >= ParentItem.MinWidth))
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                                Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) - _dbldeltaHorizontal);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - Math.Abs(_dbldeltaHorizontal * 2));
                            }
                        }
                    }
                    if (_dblVerticalChange > 0 && _dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange) && _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(-_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(-_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) + _dbldeltaVertical);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += Math.Abs(_dbldeltaHorizontal * 2);
                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    if (_dblHorizontalChange > 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }

                    else if (_dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (_dblVerticalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange > 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    break;
                case ThumbPosition.LeftCenter:
                    if (_dblHorizontalChange < 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);

                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }
                    else if (_dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case ThumbPosition.RightCenter:

                    if (_dblHorizontalChange > 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }

                    else if (_dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    break;
                case ThumbPosition.TopCenter:
                    if (_dblVerticalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange < 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }

                    break;
                case ThumbPosition.TopLeft:
                    if (_dblVerticalChange < 0 && _dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange) && _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(-_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(-_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) + _dbldeltaVertical);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += Math.Abs(_dbldeltaHorizontal * 2);
                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    if (_dblVerticalChange > 0 && _dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange) && _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if (((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight) && ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal >= ParentItem.MinWidth))
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                                Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) - _dbldeltaHorizontal);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - Math.Abs(_dbldeltaHorizontal * 2));
                            }
                        }
                    }
                    if (_dblHorizontalChange < 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);

                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }
                    else if (_dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (_dblVerticalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange < 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }

                    break;
                case ThumbPosition.TopRight:
                    if (_dblVerticalChange < 0 && _dblHorizontalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(-_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(-_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) + _dbldeltaVertical);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += Math.Abs(_dbldeltaHorizontal * 2);
                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }

                    else if (_dblVerticalChange > 0 && _dblHorizontalChange < 0)
                    {

                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);

                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight && (ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal >= ParentItem.MinWidth)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                                Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) - _dbldeltaHorizontal);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - Math.Abs(_dbldeltaHorizontal * 2));
                            }
                        }
                    }
                    if (_dblHorizontalChange > 0)
                    {
                        bool _blnflag = true;
                        for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                        {
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetRight(ParentItem, Canvas.GetRight(ParentItem) + _dbldeltaHorizontal);
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal);
                            (ParentItem as IResizeThumbOwner).ImageControlWidth += (Math.Abs(_dbldeltaHorizontal) * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }

                            if (_blnflag == false)
                                break;
                        }
                    }

                    else if (_dblHorizontalChange < 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                        {
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetRight(ParentItem, Canvas.GetTop(ParentItem) - _dbldeltaHorizontal);
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlWidth - (Math.Abs(_dbldeltaHorizontal) * 2));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (_dblVerticalChange > 0)
                    {
                        for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                        {

                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                                Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = Math.Abs((ParentItem as IResizeThumbOwner).ImageControlHeight - Math.Abs(_dbldeltaVertical * 2));
                            }
                        }
                    }
                    else if (_dblVerticalChange < 0)
                    {
                        for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                        {
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical);
                            Canvas.SetBottom(ParentItem, Canvas.GetBottom(ParentItem) - _dbldeltaVertical);
                            (ParentItem as IResizeThumbOwner).ImageControlHeight += Math.Abs(_dbldeltaVertical * 2);

                            if (CheckBoundaryConditions() == false)
                            {
                                Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                                Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                                (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                                (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                            }
                        }
                    }
                    break;
            }
        }

        protected void _DoDragDeltaUsingAngle(double _dblHorizontalChange, double _dblVerticalChange, Control desControl)
        {
            double _dbldeltaVertical, _dbldeltaHorizontal = 0;
            ContentControl _contentctrlObj = desControl as ContentControl;
            double _dblPrevLeft = Canvas.GetLeft(_contentctrlObj);
            double _dblPrevTop = Canvas.GetTop(_contentctrlObj);
            double _dblPrevHeight = _contentctrlObj.ActualHeight;
            double _dblPrevWidth = _contentctrlObj.ActualWidth;

            double allowedHorizontalChange = _dblHorizontalChange / 12;
            double allowedVerticalChange = _dblVerticalChange / 12;

            if ((_dblVerticalChange > 0 && base.VerticalAlignment == VerticalAlignment.Bottom) ||
                (_dblVerticalChange < 0 && base.VerticalAlignment == VerticalAlignment.Top))
            {
                bool _blnBoundaryConditionflag = true;
                for (int i = 0; i < Math.Abs(allowedVerticalChange); i++)
                {
                    _dblPrevLeft = Canvas.GetLeft(_contentctrlObj);
                    _dblPrevTop = Canvas.GetTop(_contentctrlObj);
                    _dblPrevHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight;
                    _dblPrevWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth;

                    switch (base.VerticalAlignment)
                    {
                        case System.Windows.VerticalAlignment.Bottom:
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + (_rendertransformOrigin.Y * _dbldeltaVertical * (1 - Math.Cos(-_dblRadianangle))));
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) - _dbldeltaVertical * _rendertransformOrigin.Y * Math.Sin(-_dblRadianangle));
                            (ParentItem as IResizeThumbOwner).ImageControlHeight -= _dbldeltaVertical;

                            break;

                        case System.Windows.VerticalAlignment.Top:
                            _dbldeltaVertical = Math.Min(-i, ParentItem.ActualHeight - ParentItem.MinHeight);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical * Math.Cos(-_dblRadianangle) + (_rendertransformOrigin.Y * _dbldeltaVertical * (1 - Math.Cos(-_dblRadianangle))));
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaVertical * Math.Sin(-_dblRadianangle) - (_rendertransformOrigin.Y * _dbldeltaVertical * Math.Sin(-_dblRadianangle)));
                            (ParentItem as IResizeThumbOwner).ImageControlHeight -= _dbldeltaVertical;
                            break;
                        default:
                            break;
                    }
                    if (CheckBoundaryConditions() == false)
                    {
                        Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                        Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                    }

                    if (_blnBoundaryConditionflag == false)
                    {
                        break;
                    }
                }
            }
            else if ((_dblVerticalChange < 0 && base.VerticalAlignment == VerticalAlignment.Bottom) ||
                     (_dblVerticalChange > 0 && base.VerticalAlignment == VerticalAlignment.Top))
            {
                bool _blnBoundaryStat = false;
                for (int _i32Val = 0; _i32Val < Math.Abs(allowedVerticalChange); _i32Val++)
                {
                    switch (base.VerticalAlignment)
                    {
                        case System.Windows.VerticalAlignment.Bottom:
                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + (_rendertransformOrigin.Y * _dbldeltaVertical * (1 - Math.Cos(-_dblRadianangle))));
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) - _dbldeltaVertical * _rendertransformOrigin.Y * Math.Sin(-_dblRadianangle));
                                (ParentItem as IResizeThumbOwner).ImageControlHeight -= _dbldeltaVertical;
                            }
                            else
                            {
                                _blnBoundaryStat = true;
                                break;
                            }
                            break;

                        case System.Windows.VerticalAlignment.Top:
                            _dbldeltaVertical = Math.Min(_i32Val, ParentItem.ActualHeight - ParentItem.MinHeight);
                            if ((ParentItem as IResizeThumbOwner).ImageControlHeight - _dbldeltaVertical >= ParentItem.MinHeight)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaVertical * Math.Cos(-_dblRadianangle) + (_rendertransformOrigin.Y * _dbldeltaVertical * (1 - Math.Cos(-_dblRadianangle))));
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaVertical * Math.Sin(-_dblRadianangle) - (_rendertransformOrigin.Y * _dbldeltaVertical * Math.Sin(-_dblRadianangle)));

                                (ParentItem as IResizeThumbOwner).ImageControlHeight -= _dbldeltaVertical;
                            }
                            else
                            {
                                _blnBoundaryStat = true;
                                break; ;
                            }
                            break;
                        default:
                            break;

                    }
                    if (CheckBoundaryConditions() == false)
                    {
                        Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                        Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                    }
                    if (_blnBoundaryStat == true)
                    {
                        break;
                    }
                }
            }
            if ((_dblHorizontalChange > 0 && base.HorizontalAlignment == HorizontalAlignment.Right) ||
                (_dblHorizontalChange < 0 && base.HorizontalAlignment == HorizontalAlignment.Left))
            {
                bool _blnBoundaryConditionflag = true;
                for (int i = 0; i < Math.Abs(allowedHorizontalChange); i++)
                {
                    _dblPrevLeft = Canvas.GetLeft(_contentctrlObj);
                    _dblPrevTop = Canvas.GetTop(_contentctrlObj);
                    _dblPrevHeight = (ParentItem as IResizeThumbOwner).ImageControlHeight;
                    _dblPrevWidth = (ParentItem as IResizeThumbOwner).ImageControlWidth;

                    switch (base.HorizontalAlignment)
                    {
                        case System.Windows.HorizontalAlignment.Left:
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaHorizontal * Math.Sin(_dblRadianangle) - _rendertransformOrigin.X * _dbldeltaHorizontal * Math.Sin(_dblRadianangle));
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal * Math.Cos(_dblRadianangle) + (_rendertransformOrigin.X * _dbldeltaHorizontal * (1 - Math.Cos(_dblRadianangle))));
                            (ParentItem as IResizeThumbOwner).ImageControlWidth -= _dbldeltaHorizontal;
                            break;
                        case System.Windows.HorizontalAlignment.Right:
                            _dbldeltaHorizontal = Math.Min(-i, ParentItem.ActualWidth - ParentItem.MinWidth);
                            Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) - _rendertransformOrigin.X * _dbldeltaHorizontal * Math.Sin(_dblRadianangle));
                            Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal * _rendertransformOrigin.X * (1 - Math.Cos(_dblRadianangle))));
                            (ParentItem as IResizeThumbOwner).ImageControlWidth -= _dbldeltaHorizontal;
                            break;
                        default:
                            break;

                    }

                    if (CheckBoundaryConditions() == false)
                    {
                        Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                        Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                    }

                    if (_blnBoundaryConditionflag == false)
                        break;
                }

            }
            else if ((_dblHorizontalChange < 0 && base.HorizontalAlignment == HorizontalAlignment.Right) ||
                     (_dblHorizontalChange > 0 && base.HorizontalAlignment == HorizontalAlignment.Left))
            {
                bool _blnBoundaryStat = false;
                for (int _i32Val = 0; _i32Val < Math.Abs(allowedHorizontalChange); _i32Val++)
                {
                    switch (base.HorizontalAlignment)
                    {
                        case System.Windows.HorizontalAlignment.Left:
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal >= ParentItem.MinWidth)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + _dbldeltaHorizontal * Math.Sin(_dblRadianangle) - _rendertransformOrigin.X * _dbldeltaHorizontal * Math.Sin(_dblRadianangle));
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + _dbldeltaHorizontal * Math.Cos(_dblRadianangle) + (_rendertransformOrigin.X * _dbldeltaHorizontal * (1 - Math.Cos(_dblRadianangle))));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth -= _dbldeltaHorizontal;
                            }
                            else
                            {
                                _blnBoundaryStat = true;
                                break;
                            }
                            break;

                        case System.Windows.HorizontalAlignment.Right:
                            _dbldeltaHorizontal = Math.Min(_i32Val, ParentItem.ActualWidth - ParentItem.MinWidth);
                            if ((ParentItem as IResizeThumbOwner).ImageControlWidth - _dbldeltaHorizontal > ParentItem.MinWidth)
                            {
                                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) - _rendertransformOrigin.X * _dbldeltaHorizontal * Math.Sin(_dblRadianangle));
                                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + (_dbldeltaHorizontal * _rendertransformOrigin.X * (1 - Math.Cos(_dblRadianangle))));
                                (ParentItem as IResizeThumbOwner).ImageControlWidth -= _dbldeltaHorizontal;
                            }
                            else
                            {
                                _blnBoundaryStat = true;
                                break; ;
                            }
                            break;
                        default:
                            break;

                    }
                    if (CheckBoundaryConditions() == false)
                    {
                        Canvas.SetLeft(_contentctrlObj, _dblPrevLeft);
                        Canvas.SetTop(_contentctrlObj, _dblPrevTop);
                        (ParentItem as IResizeThumbOwner).ImageControlWidth = _dblPrevWidth;
                        (ParentItem as IResizeThumbOwner).ImageControlHeight = _dblPrevHeight;
                    }
                    if (_blnBoundaryStat == true)
                    {
                        break;
                    }
                }
            }
        }

        private bool CheckBoundaryConditions()
        {
            bool _blnResizeFlag = true;
            ContentControl _contentctrlObj = this.DataContext as ContentControl;
            double _dblLeft, _dblTop, _dblRight, _dblBottom;

            _dblLeft = Canvas.GetLeft(ParentItem);
            _dblTop = Canvas.GetTop(ParentItem);
            _dblRight = Canvas.GetLeft(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlWidth;
            _dblBottom = Canvas.GetTop(ParentItem) + (ParentItem as IResizeThumbOwner).ImageControlHeight;

            Point _testPt = new Point();
            _testPt.X = (_dblLeft + _dblRight) / 2;
            _testPt.Y = (_dblTop + _dblBottom) / 2;


            double _dblHeight = 0;
            double _dblWidth = 0;
            if (_contentctrlObj.Parent != null)
            {
                _dblHeight = ((System.Windows.FrameworkElement)((_contentctrlObj.Parent))).ActualHeight;
                _dblWidth = ((System.Windows.FrameworkElement)((_contentctrlObj.Parent))).ActualWidth;
            }


            for (int i = 0; i < 1; i++)
            {
                if (_testPt.X < 0)
                {
                    _blnResizeFlag = false;
                    break;
                }
                if (_testPt.Y < 0)
                {
                    _blnResizeFlag = false;
                    break;
                }
                if (_testPt.X >= _dblWidth)
                {
                    _blnResizeFlag = false;
                    break;
                }
                if (_testPt.Y >= _dblHeight)
                {
                    _blnResizeFlag = false;
                    break;
                }
            }
            return _blnResizeFlag;
        }

        #endregion

        public void UnregisterEvent()
        {
            this.DragDelta -= new DragDeltaEventHandler(ResizeThumb_DragDelta);
            this.DragCompleted -= new DragCompletedEventHandler(ResizeThumb_DragCompleted);
            this.DragStarted -= new DragStartedEventHandler(ResizeThumb_DragStarted);
            this.MouseEnter -= new MouseEventHandler(ResizeThumb_MouseEnter);

        }
    }
}
