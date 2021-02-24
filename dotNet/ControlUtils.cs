using Atomus.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Atomus.Windows.Style
{
    #region Image Property
    /// <summary>
    /// 컨트롤 확장 dp
    /// </summary>
    public class CoEx
    {
        /// <summary>
        /// Normal 이미지
        /// </summary>
        public static readonly DependencyProperty Image_N_Property =
            DependencyProperty.RegisterAttached("Image_N", typeof(BitmapImage), typeof(CoEx), new PropertyMetadata(default(BitmapImage)));
        public static void SetImage_N(UIElement element, BitmapImage value)
        {
            element.SetValue(Image_N_Property, value);
        }
        public static double GetImage_N(UIElement element)
        {
            return (double)element.GetValue(Image_N_Property);
        }

        /// <summary>
        /// Over 이미지
        /// </summary>
        public static readonly DependencyProperty Image_O_Property =
            DependencyProperty.RegisterAttached("Image_O", typeof(BitmapImage), typeof(CoEx), new PropertyMetadata(default(BitmapImage)));
        public static void SetImage_O(UIElement element, BitmapImage value)
        {
            element.SetValue(Image_O_Property, value);
        }
        public static double GetImage_O(UIElement element)
        {
            return (double)element.GetValue(Image_O_Property);
        }

        /// <summary>
        /// Selected 이미지
        /// </summary>
        public static readonly DependencyProperty Image_S_Property =
            DependencyProperty.RegisterAttached("Image_S", typeof(BitmapImage), typeof(CoEx), new PropertyMetadata(default(BitmapImage)));
        public static void SetImage_S(UIElement element, BitmapImage value)
        {
            element.SetValue(Image_S_Property, value);
        }
        public static double GetImage_S(UIElement element)
        {
            return (double)element.GetValue(Image_S_Property);
        }

        /// <summary>
        /// Dimmed 이미지
        /// </summary>
        public static readonly DependencyProperty Image_D_Property =
            DependencyProperty.RegisterAttached("Image_D", typeof(BitmapImage), typeof(CoEx), new PropertyMetadata(default(BitmapImage)));
        public static void SetImage_D(UIElement element, BitmapImage value)
        {
            element.SetValue(Image_D_Property, value);
        }
        public static double GetImage_D(UIElement element)
        {
            return (double)element.GetValue(Image_D_Property);
        }

        public static readonly DependencyProperty MinimizeCommandProperty =
            DependencyProperty.Register("MinimizeCommand", typeof(ICommand), typeof(CoEx));
        public static void SetMinimizeCommand(UIElement element, ICommand value)
        {
            element.SetValue(MinimizeCommandProperty, value);
        }
        public static double GetMinimizeCommand(UIElement element)
        {
            return (double)element.GetValue(MinimizeCommandProperty);
        }
    }
    #endregion

    #region Extensions
    public static class ControlExtensions
    {
        public static void SetOneTimeValidation(this TextBox textbox, ValidationRule rule)
        {
            try
            {
                textbox.GetBindingExpression(TextBox.TextProperty).ParentBinding.ValidationRules.Add(rule);
                textbox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                textbox.GetBindingExpression(TextBox.TextProperty).ParentBinding.ValidationRules.Clear();
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
            }
        }
    }

    public static class PrimitiveExtensions
    {
        public static string ToStringEAI(this string text)
        {
            if (text.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }
            else
            {
                return text.Trim();
            }
        }
    }
    #endregion

    #region ValidationRules
    public class EmptyStringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                value = "";

            string valueToValidate = value as string;
            if (valueToValidate.IsNullOrWhiteSpace())
            {
                return new ValidationResult(false, "필수 데이터 입력");
            }

            return new ValidationResult(true, null);
        }
    }
    #endregion

    #region TriggerAction
    public class EnterKeyDownAction : TriggerAction<Button>
    {
        public static readonly DependencyProperty TargetProperty =
                DependencyProperty.Register("Target", typeof(TextBox), typeof(EnterKeyDownAction), new UIPropertyMetadata(null));
        public TextBox Target
        {
            get { return (TextBox)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (Target != null)
            {
                Target.Focus();
                var key = Key.Enter;
                var routedEvent = Keyboard.KeyDownEvent;
                Target.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(Target), 0, key) { RoutedEvent = routedEvent });
            }
        }
    }
    #endregion

    #region Behavior

    public class StackPanelButtonCollapseBehavior : Behavior<StackPanel>
    {
        public Control.IAction Core
        {
            get { return (Control.IAction)GetValue(CoreProperty); }
            set { SetValue(CoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoreProperty =
            DependencyProperty.Register("Core", typeof(Control.IAction), typeof(StackPanelButtonCollapseBehavior), new PropertyMetadata(null));

        public StackPanelButtonCollapseBehavior()
        {
        }

        protected override void OnAttached()
        {
            try
            {
                if (this.AssociatedObject.Children.Count > 0)
                {
                    Button btnTemp = null;
                    foreach (var item in this.AssociatedObject.Children)
                    {
                        if (item is Button)
                        {
                            btnTemp = item as Button;

                            if (btnTemp.Content.Equals("검색") || btnTemp.Content.Equals("Search"))
                            {
                                CollapseButton(btnTemp, "Action.Search");
                            }
                            else if (btnTemp.Content.Equals("저장") || btnTemp.Content.Equals("Save"))
                            {
                                CollapseButton(btnTemp, "Action.Save");
                            }
                            else if (btnTemp.Content.Equals("삭제") || btnTemp.Content.Equals("Delete"))
                            {
                                CollapseButton(btnTemp, "Action.Delete");
                            }
                            else if (btnTemp.Content.Equals("출력") || btnTemp.Content.Equals("Print"))
                            {
                                CollapseButton(btnTemp, "Action.Print");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
            }
            finally
            {
                base.OnAttached();
            }
        }

        private void CollapseButton(Button btnTemp, string strAction = "Action.Search")
        {
            try
            {
                if ((Core as Control.IAction).GetAttribute(strAction) == "Y")
                {
                    btnTemp.Visibility = Visibility.Visible;
                }
                else
                {
                    btnTemp.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
                btnTemp.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnDetaching()
        {
            try
            {
                Core = null;
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
            }
            finally
            {
                base.OnDetaching();
            }
        }
    }

    public class GridControlContextMenuBehavior : Behavior<DevExpress.Xpf.Grid.GridControl>
    {
        /// <summary>
        /// [1] visible [0] disable [-1] remove
        /// 엑셀저장    엑셀저장열기  출력  복사  행추가 행삭제 행복사 Sum Avg
        /// 1,          1,              1,  1,      1,      1,  1,      1,  1
        /// </summary>
        public string ItemVisible
        {
            get { return (string)GetValue(ItemVisibleProperty); }
            set { SetValue(ItemVisibleProperty, value); }
        }

        public static readonly DependencyProperty ItemVisibleProperty =
            DependencyProperty.Register("ItemVisible", typeof(string), typeof(GridControlContextMenuBehavior), new PropertyMetadata("1,1,1,1,1,1,1,1,1"));

        public object PassArgument
        {
            get { return (object)GetValue(PassArgumentProperty); }
            set { SetValue(PassArgumentProperty, value); }
        }

        public static readonly DependencyProperty PassArgumentProperty =
            DependencyProperty.Register("PassArgument", typeof(object), typeof(GridControlContextMenuBehavior), new PropertyMetadata("1"));

        /// <summary>
        /// 엑셀저장
        /// </summary>
        public ICommand MenuExportExcelCommand
        {
            get { return (ICommand)GetValue(MenuExportExcelCommandProperty); }
            set { SetValue(MenuExportExcelCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuExportExcelCommandProperty =
            DependencyProperty.Register(nameof(MenuExportExcelCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuExportExcelProcess()
        {
            SaveExcel(this.AssociatedObject, false);
        }

        /// <summary>
        /// 엑셀저장 열기
        /// </summary>
        public ICommand MenuExportExcelOpenCommand
        {
            get { return (ICommand)GetValue(MenuExportExcelOpenCommandProperty); }
            set { SetValue(MenuExportExcelOpenCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuExportExcelOpenCommandProperty =
            DependencyProperty.Register(nameof(MenuExportExcelOpenCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuExportExcelOpenProcess()
        {
            SaveExcel(this.AssociatedObject, true);
        }

        /// <summary>
        /// 출력
        /// </summary>
        public ICommand MenuPrintOutCommand
        {
            get { return (ICommand)GetValue(MenuPrintOutCommandProperty); }
            set { SetValue(MenuPrintOutCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuPrintOutCommandProperty =
            DependencyProperty.Register(nameof(MenuPrintOutCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuPrintOutProcess()
        {
            this.AssociatedObject.View.ShowPrintPreviewDialog(Application.Current.MainWindow);
        }

        /// <summary>
        /// 복사
        /// </summary>
        public ICommand MenuCopyCellCommand
        {
            get { return (ICommand)GetValue(MenuCopyCellCommandProperty); }
            set { SetValue(MenuCopyCellCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuCopyCellCommandProperty =
            DependencyProperty.Register(nameof(MenuCopyCellCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuCopyCellProcess()
        {
            DevExpress.Xpf.Grid.TableView dxTv = this.AssociatedObject.View as DevExpress.Xpf.Grid.TableView;
            //dxTv.CopyCellsToClipboard(dxTv.FocusedRowHandle, (DevExpress.Xpf.Grid.GridColumn)dxTv.Grid.CurrentColumn, dxTv.FocusedRowHandle, (DevExpress.Xpf.Grid.GridColumn)dxTv.Grid.CurrentColumn);    // 셀 데이터(컬럼명 포함)
            //dxTv.CopySelectedCellsToClipboard();  // 선택된 셀 전체(컬럼명 포함), 행이 선택되면 행 전체(컬럼명 포함)
            Clipboard.SetText(dxTv.Grid.GetFocusedValue().ToString());  // 셀 데이터만(컬럼명 제외)
        }

        /// <summary>
        /// 행추가
        /// </summary>
        public ICommand MenuAddRowCommand
        {
            get { return (ICommand)GetValue(MenuAddRowCommandProperty); }
            set { SetValue(MenuAddRowCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuAddRowCommandProperty =
            DependencyProperty.Register(nameof(MenuAddRowCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuAddRowProcess(object x)
        {
            // 개별구현
        }

        /// <summary>
        /// 행삭제 
        /// </summary>
        public ICommand MenuRemoveRowCommand
        {
            get { return (ICommand)GetValue(MenuRemoveRowCommandProperty); }
            set { SetValue(MenuRemoveRowCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuRemoveRowCommandProperty =
            DependencyProperty.Register(nameof(MenuRemoveRowCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuRemoveRowProcess(object x)
        {
            // 개별구현
        }

        /// <summary>
        /// 행복사
        /// </summary>
        public ICommand MenuCopyRowCommand
        {
            get { return (ICommand)GetValue(MenuCopyRowCommandProperty); }
            set { SetValue(MenuCopyRowCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuCopyRowCommandProperty =
            DependencyProperty.Register(nameof(MenuCopyRowCommand), typeof(ICommand), typeof(GridControlContextMenuBehavior));

        private void MenuCopyRowProcess(object x)
        {
            // 개별구현
        }

        private double MenuSumProcess()
        {
            try
            {
                if (this.AssociatedObject.SelectionMode == DevExpress.Xpf.Grid.MultiSelectMode.Cell)
                {
                    DevExpress.Xpf.Grid.TableView dxTv = this.AssociatedObject.View as DevExpress.Xpf.Grid.TableView;
                    List<DevExpress.Xpf.Grid.GridCell> lst = dxTv.GetSelectedCells().ToList();
                    double sum = 0.0f;
                    foreach (DevExpress.Xpf.Grid.GridCell item in lst)
                    {
                        double val;
                        if (double.TryParse(item.Value.ToString(), out val))
                        {
                            sum += val;
                        }
                    }
                    return sum;
                }
                else
                {
                    DevExpress.Xpf.Grid.TableView dxTv = this.AssociatedObject.View as DevExpress.Xpf.Grid.TableView;
                    List<DevExpress.Xpf.Grid.GridRowInfo> selectedRows = dxTv.GetSelectedRows().ToList();
                    double sum = 0.0f;
                    foreach (DevExpress.Xpf.Grid.GridRowInfo rowInfo in selectedRows)
                    {
                        if (rowInfo.RowHandle >= 0)
                        {
                            foreach (DevExpress.Xpf.Grid.GridColumn col in this.AssociatedObject.Columns)
                            {
                                if (!col.Visible)
                                    continue;

                                object cellValue = this.AssociatedObject.GetCellValue(rowInfo.RowHandle, col);
                                if (cellValue == null)
                                    continue;

                                double val;
                                if (double.TryParse(cellValue.ToString(), out val))
                                {
                                    sum += val;
                                }
                            }
                        }
                    }
                    return sum;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
                return 0;
            }
        }

        private double MenuAvgProcess()
        {
            try
            {
                if (this.AssociatedObject.SelectionMode == DevExpress.Xpf.Grid.MultiSelectMode.Cell)
                {
                    DevExpress.Xpf.Grid.TableView dxTv = this.AssociatedObject.View as DevExpress.Xpf.Grid.TableView;
                    List<DevExpress.Xpf.Grid.GridCell> lst = dxTv.GetSelectedCells().ToList();
                    double sum = 0.0f;
                    int count = 0;
                    foreach (DevExpress.Xpf.Grid.GridCell item in lst)
                    {
                        double val;
                        if (double.TryParse(item.Value.ToString(), out val))
                        {
                            count++;
                            sum += val;
                        }
                    }

                    if (count == 0)
                        return sum;
                    else
                        return sum / count;
                }
                else
                {
                    DevExpress.Xpf.Grid.TableView dxTv = this.AssociatedObject.View as DevExpress.Xpf.Grid.TableView;
                    List<DevExpress.Xpf.Grid.GridRowInfo> selectedRows = dxTv.GetSelectedRows().ToList();
                    double sum = 0.0f;
                    int count = 0;
                    foreach (DevExpress.Xpf.Grid.GridRowInfo rowInfo in selectedRows)
                    {
                        if (rowInfo.RowHandle >= 0)
                        {
                            foreach (DevExpress.Xpf.Grid.GridColumn col in this.AssociatedObject.Columns)
                            {
                                if (!col.Visible)
                                    continue;

                                object cellValue = this.AssociatedObject.GetCellValue(rowInfo.RowHandle, col);
                                if (cellValue == null)
                                    continue;

                                double val;
                                if (double.TryParse(cellValue.ToString(), out val))
                                {
                                    count++;
                                    sum += val;
                                }
                            }
                        }
                    }

                    if (count == 0)
                        return sum;
                    else
                        return sum / count;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
                return 0;
            }
        }

        public GridControlContextMenuBehavior()
        {
            MenuExportExcelCommand = new MVVM.DelegateCommand(MenuExportExcelProcess);
            MenuExportExcelOpenCommand = new MVVM.DelegateCommand(MenuExportExcelOpenProcess);
            MenuPrintOutCommand = new MVVM.DelegateCommand(MenuPrintOutProcess);
            MenuCopyCellCommand = new MVVM.DelegateCommand(MenuCopyCellProcess);
            MenuAddRowCommand = new MVVM.DelegateCommand((object x) => { this.MenuAddRowProcess(x); }, () => true);
            MenuRemoveRowCommand = new MVVM.DelegateCommand((object x) => { this.MenuRemoveRowProcess(x); }, () => true);
            MenuCopyRowCommand = new MVVM.DelegateCommand((object x) => { this.MenuCopyRowProcess(x); }, () => true);
        }

        protected override void OnAttached()
        {
            try
            {
                string[] visibleStates = ItemVisible.Split(',');
                int count = 0;
                ContextMenu context = new ContextMenu();
                context.Style = (System.Windows.Style)Application.Current.FindResource("ContextMenu_Base");
                MenuItem mItem;
                Separator sep;

                mItem = new MenuItem
                {
                    Header = "엑셀저장",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuExportExcelCommand
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "엑셀저장 열기",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuExportExcelOpenCommand
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "출력",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuPrintOutCommand
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "복사",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuCopyCellCommand
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);


                sep = new Separator
                {
                    Style = (System.Windows.Style)Application.Current.FindResource("Separator_Menu_Base")
                };
                context.Items.Add(sep);


                count++;
                mItem = new MenuItem
                {
                    Header = "행추가",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuAddRowCommand,
                    CommandParameter = this.PassArgument,
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "행삭제",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuRemoveRowCommand,
                    CommandParameter = this.PassArgument,
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "행복사",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                    Command = MenuCopyRowCommand,
                    CommandParameter = this.PassArgument,
                };
                context.Items.Add(mItem);

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);


                sep = new Separator
                {
                    Style = (System.Windows.Style)Application.Current.FindResource("Separator_Menu_Base")
                };
                context.Items.Add(sep);


                count++;
                mItem = new MenuItem
                {
                    Header = "Sum",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                };
                context.Items.Add(mItem);
                sumMenuItem = mItem;

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);

                count++;
                mItem = new MenuItem
                {
                    Header = "Avg",
                    Template = (ControlTemplate)Application.Current.FindResource("MenuItemControlTemplate_Base"),
                };
                context.Items.Add(mItem);
                avgMenuItem = mItem;

                if (visibleStates[count].ToInt() == 1)
                    mItem.IsEnabled = true;
                else if (visibleStates[count].ToInt() == 0)
                    mItem.IsEnabled = false;
                else
                    context.Items.Remove(mItem);


                this.AssociatedObject.ContextMenuOpening += Context_ContextMenuOpening;
                this.AssociatedObject.ContextMenu = context;
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
            }
            finally
            {
                base.OnAttached();
            }
        }

        private MenuItem sumMenuItem, avgMenuItem;
        private void Context_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sumMenuItem != null)
            {
                sumMenuItem.Header = "Sum : " + MenuSumProcess();
            }

            if (avgMenuItem != null)
            {
                avgMenuItem.Header = "Avg : " + MenuAvgProcess();
            }
        }

        protected override void OnDetaching()
        {
            this.sumMenuItem = null;
            this.avgMenuItem = null;

            this.AssociatedObject.ContextMenuOpening -= Context_ContextMenuOpening;
            this.AssociatedObject.ContextMenu = null;

            base.OnDetaching();
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="gridControl"></param>
        /// <param name="runAfter"></param>
        private void SaveExcel(DevExpress.Xpf.Grid.GridControl gridControl, bool runAfter = false)
        {
            System.Windows.Forms.SaveFileDialog fileDialog;
            fileDialog = new System.Windows.Forms.SaveFileDialog()
            {
                DefaultExt = "*.xlsx",
                Filter = "xlsx Files(.xlsx)|*.xlsx|xls files (*.xls)|*.xls|All files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gridControl.View.ExportToXlsx(fileDialog.FileName);
                if (runAfter)
                {
                    System.Diagnostics.Process.Start(fileDialog.FileName);
                }
            }
        }
    }
    #endregion

    #region PasswordBox
    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
    #endregion

    #region ContextMenuItemExtension
    public class ContextMenuItemExtensions : DependencyObject
    {
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.RegisterAttached("GroupName",
                                         typeof(String),
                                         typeof(ContextMenuItemExtensions),
                                         new PropertyMetadata(String.Empty, OnGroupNameChanged));

        public static void SetGroupName(MenuItem element, String value)
        {
            element.SetValue(GroupNameProperty, value);
        }

        public static String GetGroupName(MenuItem element)
        {
            return element.GetValue(GroupNameProperty).ToString();
        }

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = d as MenuItem;
            if (menuItem != null)
            {
                menuItem.Click += MenuItem_Click;
            }
        }

        private static void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var item = e.OriginalSource as MenuItem;
            //if (item != null)
            //{   
            //    DevExpress.Xpf.Grid.GridControl grid = TreeHelper.TryFindParent<DevExpress.Xpf.Grid.GridControl>(item);
            //    if (grid is DevExpress.Xpf.Grid.GridControl)
            //    {

            //    }
            //}
        }
    }
    #endregion    

    #region Find Parent Control
    /// <summary>
    /// Helper methods for UI-related tasks.
    /// </summary>
    public static class TreeHelper
    {
        #region find parent

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent == null && frameworkElement is System.Windows.Controls.Primitives.Popup)
                    parent = (frameworkElement as System.Windows.Controls.Primitives.Popup).PlacementTarget;

                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        #endregion

        #region find children

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given
        /// type that are descendants of the <paramref name="source"/> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">The root element that marks the source of the search. If the
        /// source is already of the requested type, it will not be included in the result.</param>
        /// <returns>All descendants of <paramref name="source"/> that match the requested type.</returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject source) where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source);
                foreach (DependencyObject child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }


        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetChild"/> method, which also
        /// supports content elements. Keep in mind that for content elements,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
        {
            if (parent == null) yield break;

            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject)obj;
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }

        #endregion

        #region find from point

        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindFromPoint<T>(UIElement reference, Point point)
            where T : DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point) as DependencyObject;

            if (element == null) return null;
            else if (element is T) return (T)element;
            else return TryFindParent<T>(element);
        }

        #endregion
    }
    #endregion    

    #region Converter
    /// <summary>
    /// WindowState To Visible Converter
    /// </summary>
    public class WindowStateToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibleFlag = Visibility.Collapsed;

            if (value == null)
                return visibleFlag;

            if (value is WindowState)
            {
                switch ((WindowState)value)
                {
                    case WindowState.Maximized:
                        visibleFlag = Visibility.Visible;
                        break;
                    case WindowState.Minimized:
                        visibleFlag = Visibility.Collapsed;
                        break;
                    case WindowState.Normal:
                        visibleFlag = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
            return visibleFlag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Null to True Converter
    /// </summary>
    public class IsNullToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }

    /// <summary>
    /// String To Int Converter
    /// </summary>
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                if (parameter == null)
                {
                    double ret;
                    if (double.TryParse(value.ToString(), out ret))
                        return (int)ret;
                    else
                        return 0;
                }
                else if (parameter is int)
                {
                    double ret;
                    if (double.TryParse(value.ToString(), out ret))
                        return (int)ret * (int)parameter / 100;
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Visible to Collapsed Converter
    /// </summary>
    public class VisibleToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibleFlag = Visibility.Collapsed;

            if (value == null)
                return visibleFlag;

            if (value is Visibility)
            {
                switch ((Visibility)value)
                {
                    case Visibility.Collapsed:
                        visibleFlag = Visibility.Visible;
                        break;
                    case Visibility.Hidden:
                    case Visibility.Visible:
                        visibleFlag = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
            return visibleFlag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool To Visible Converter
    /// </summary>
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibleFlag = Visibility.Collapsed;

            if (value == null)
                return visibleFlag;

            bool bReverse = false;
            if (parameter != null && parameter.ToString() == "1")
                bReverse = true;

            if (value is bool)
            {
                bool val = (bool)value;
                if (bReverse)
                    val = !val;

                switch (val)
                {
                    case true:
                        visibleFlag = Visibility.Visible;
                        break;

                    case false:
                        visibleFlag = Visibility.Collapsed;
                        break;
                }
            }
            return visibleFlag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Color To Brush Converter
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is Color)
            {
                return new SolidColorBrush((Color)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Empty To Disable Converter
    /// </summary>
    public class EmptyToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ret = true;

            if (value == null || value.ToString() == "")
                ret = false;

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || parameter.ToString() == "")
                return false;

            return ((int)value) > int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Empty To Disable Converter
    /// </summary>
    public class SameValue10ToTrueFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (value.ToString() == parameter.ToString())
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            if ((bool)value)
                return 1;
            else
                return 0;

        }
    }

    /// <summary>
    /// YN Converter
    /// </summary>
    public class SameValueYNToTrueFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (value.ToString() == parameter.ToString())
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "N";

            if ((bool)value)
                return "Y";
            else
                return "N";

        }
    }

    /// <summary>
    /// Empty To Disable Converter
    /// </summary>
    public class SameValueToVisibleHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Hidden;

            if (value.ToString() == parameter.ToString())
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EqualParameterAndBoolToConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0].ToString().Contains(parameter.ToString()) && (bool)values[1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class GaugeValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
            {
                return 0;
            }
            else
            {
                double data, height;

                if (double.TryParse(values[0].ToString(), out data) &&
                    double.TryParse(values[1].ToString(), out height))
                {
                    return (double)(data * height / 100);
                }
                else
                {
                    return 0;
                }
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.OfType<IConvertible>().All(System.Convert.ToBoolean);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class BooleanOrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.OfType<IConvertible>().Any(System.Convert.ToBoolean);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class BooleanOrToVisibilityConverter : IMultiValueConverter
    {
        public Visibility HiddenVisibility { get; set; }

        public bool IsInverted { get; set; }

        public BooleanOrToVisibilityConverter()
        {
            HiddenVisibility = Visibility.Collapsed;
            IsInverted = false;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = values.OfType<IConvertible>().Any(System.Convert.ToBoolean);
            if (IsInverted) flag = !flag;
            return flag ? Visibility.Visible : HiddenVisibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BooleanAndToVisibilityConverter : IMultiValueConverter
    {
        public Visibility HiddenVisibility { get; set; }

        public bool IsInverted { get; set; }

        public BooleanAndToVisibilityConverter()
        {
            HiddenVisibility = Visibility.Collapsed;
            IsInverted = false;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = values.OfType<IConvertible>().All(System.Convert.ToBoolean);
            if (IsInverted) flag = !flag;
            return flag ? Visibility.Visible : HiddenVisibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EqualValueToTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
                return false;

            if (values[0].ToString().IsNullOrWhiteSpace() || values[1].ToString().IsNullOrWhiteSpace())
                return false;

            try
            {
                var v_net_qty = values[0].ToString().Trim().Replace(",", "").ToDecimal();
                var v_kalab = values[1].ToString().Trim().Equals("") ? 0M : values[1].ToString().Trim().Replace(",", "").ToDecimal();

                if (v_net_qty == v_kalab)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    #endregion
}
