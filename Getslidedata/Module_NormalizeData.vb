Module Module_NormalizeData
    Public Function NormalizeData(dt As DataTable) As DataTable
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return dt
        End If

        ' 创建新DataTable保留原始结构
        Dim normalizedDt As DataTable = dt.Clone()

        ' 计算最小值
        Dim minX As Double = dt.AsEnumerable().Min(Function(r) r.Field(Of Double)("X"))

        ' Y1、Y2、YW使用同一个最小值（三列中的最小值）
        Dim minY1 As Double = dt.AsEnumerable().Min(Function(r) r.Field(Of Double)("Y1"))
        Dim minY2 As Double = dt.AsEnumerable().Min(Function(r) r.Field(Of Double)("Y2"))
        'Dim minYw As Double = dt.AsEnumerable().Min(Function(r) r.Field(Of Double)("YW"))
        Dim minY As Double = Math.Min(minY1, minY2) ' 取三者的最小值

        ' 标准化数据
        Dim num_temp As Integer = 0
        For Each row As DataRow In dt.Rows

            Dim newRow As DataRow = normalizedDt.NewRow()
            'newRow("ID") = row.Field(Of String)("ID")
            newRow("X") = row.Field(Of Double)("X") - minX
            newRow("Y1") = row.Field(Of Double)("Y1") - minY
            newRow("Y2") = row.Field(Of Double)("Y2") - minY
            newRow("h") = row.Field(Of Double)("h")
            newRow("dh") = row.Field(Of Double)("dh")
            'If num_temp > 0 Then
            '    newRow("L") = row.Field(Of Double)("h")
            'End If
            'newRow("YW") = row.Field(Of Double)("YW") - minY

            'If row.IsNull("ID") OrElse String.IsNullOrEmpty(row.Field(Of String)("ID")) Then
            '    newRow("ID") = num_temp
            '    'ElseIf row.Field(Of String)("ID").ToString.Contains("*") = True Then
            '    '    newRow("ID") = num_temp.ToString & "*"
            'Else
            '    newRow("ID") = num_temp
            'End If
            newRow("ID") = num_temp
            num_temp = num_temp + 1 '第一条线从0而不是1开始标
            normalizedDt.Rows.Add(newRow)
        Next

        Return normalizedDt
    End Function
End Module
