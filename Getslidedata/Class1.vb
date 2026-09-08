Imports NPOI.HSSF.UserModel ' 用于.xls格式
Imports NPOI.XSSF.UserModel ' 用于.xlsx格式
Imports NPOI.SS.UserModel
Imports System.Data
Imports System.IO

Public Class ExcelExporter
    ''' <summary>
    ''' 将DataTable导出为Excel文件
    ''' </summary>
    ''' <param name="dt">要导出的DataTable</param>
    ''' <param name="filePath">保存路径（包含文件名）</param>
    ''' <param name="sheetName">工作表名称</param>
    ''' <returns>是否导出成功</returns>
    Public Function ExportDataTableToExcel(dt As DataTable, filePath As String, Optional sheetName As String = "Sheet1") As Boolean
        Try
            ' 检查文件路径是否有效
            If String.IsNullOrEmpty(filePath) Then
                Throw New ArgumentException("文件路径不能为空")
            End If

            ' 检查DataTable是否有数据
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Throw New ArgumentException("DataTable为空或没有数据")
            End If

            ' 根据文件扩展名创建相应的工作簿
            Dim workbook As IWorkbook
            Dim fileExtension = Path.GetExtension(filePath).ToLower()

            If fileExtension = ".xlsx" Then
                workbook = New XSSFWorkbook() ' 2007及以上格式
            ElseIf fileExtension = ".xls" Then
                workbook = New HSSFWorkbook() ' 2003格式
            Else
                Throw New ArgumentException("不支持的文件格式，仅支持.xls和.xlsx")
            End If

            ' 创建工作表
            Dim sheet As ISheet = workbook.CreateSheet(sheetName)

            ' 创建表头行
            Dim headerRow As IRow = sheet.CreateRow(0)

            ' 填充表头
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim cell As ICell = headerRow.CreateCell(i)
                cell.SetCellValue(dt.Columns(i).ColumnName)

                ' 设置表头样式（可选）
                Dim headerStyle As ICellStyle = workbook.CreateCellStyle()
                Dim font As IFont = workbook.CreateFont()
                font.Boldweight = CShort(FontBoldWeight.Bold)
                headerStyle.SetFont(font)
                cell.CellStyle = headerStyle
            Next

            ' 填充数据行
            For rowIndex As Integer = 0 To dt.Rows.Count - 1
                Dim dataRow As IRow = sheet.CreateRow(rowIndex + 1) ' +1 是因为第一行是表头

                For colIndex As Integer = 0 To dt.Columns.Count - 1
                    Dim cell As ICell = dataRow.CreateCell(colIndex)

                    ' 根据数据类型设置单元格值
                    Select Case dt.Columns(colIndex).DataType
                        Case GetType(String)
                            cell.SetCellValue(If(dt.Rows(rowIndex)(colIndex) Is DBNull.Value, "", dt.Rows(rowIndex)(colIndex).ToString()))
                        Case GetType(Integer), GetType(Double), GetType(Decimal)
                            If Not dt.Rows(rowIndex)(colIndex) Is DBNull.Value Then
                                cell.SetCellValue(CDbl(dt.Rows(rowIndex)(colIndex)))
                            End If
                        Case GetType(DateTime)
                            If Not dt.Rows(rowIndex)(colIndex) Is DBNull.Value Then
                                cell.SetCellValue(CDate(dt.Rows(rowIndex)(colIndex)))
                            End If
                        Case GetType(Boolean)
                            If Not dt.Rows(rowIndex)(colIndex) Is DBNull.Value Then
                                cell.SetCellValue(CBool(dt.Rows(rowIndex)(colIndex)))
                            End If
                        Case Else
                            cell.SetCellValue(If(dt.Rows(rowIndex)(colIndex) Is DBNull.Value, "", dt.Rows(rowIndex)(colIndex).ToString()))
                    End Select
                Next
            Next

            ' 自动调整列宽
            For i As Integer = 0 To dt.Columns.Count - 1
                sheet.AutoSizeColumn(i)
                ' 适当调整自动列宽，避免过窄
                If sheet.GetColumnWidth(i) > 255 * 256 Then
                    sheet.SetColumnWidth(i, 255 * 256)
                End If
            Next

            ' 保存文件
            Using fs As New FileStream(filePath, FileMode.Create)
                workbook.Write(fs)
                fs.Flush()
            End Using

            Return True
        Catch ex As Exception
            ' 处理异常，可以根据需要修改
            Console.WriteLine("导出Excel失败: " & ex.Message)
            Return False
        End Try
    End Function
End Class