Module Module_process_data
    Public Function Process_data(ByVal dtt As DataTable) As DataTable

        Dim dt_hp_temp As System.Data.DataTable = get_dt_hp()
        For k = 0 To dtt.Rows.Count - 2

            Dim dr As DataRow = dt_hp_temp.NewRow

            Dim dx As Double = Math.Abs(dtt.Rows(k)("x") - dtt.Rows(k + 1)("x"))
            Dim dy As Double = dtt.Rows(k)("y1") - dtt.Rows(k + 1)("y1") '不可abs ，否则影响倾角计算正负
            dr("L") = Math.Sqrt(dx ^ 2 + dy ^ 2)
            dr("angle") = Math.Atan(dy / dx) * 180 / Math.PI

           


            Dim ha_temp, hb_temp As Double
            If String.IsNullOrEmpty(dtt.Rows(k)("yw").ToString) = False Then
                hb_temp = dtt.Rows(k)("yw") - dtt.Rows(k)("y1")
            End If
            If String.IsNullOrEmpty(dtt.Rows(k + 1)("yw").ToString) = False Then
                ha_temp = dtt.Rows(k + 1)("yw") - dtt.Rows(k + 1)("y1")
            End If
            dr("Ha") = ha_temp
            dr("Hb") = hb_temp

            Dim dx2 As Double = Math.Abs(dtt.Rows(k)("x") - dtt.Rows(k + 1)("x"))
            Dim dy2 As Double = dtt.Rows(k)("yw") - dtt.Rows(k + 1)("yw") '不可abs ，否则影响倾角计算正负
            dr("angle_B") = Math.Atan(dy2 / dx2) * 180 / Math.PI

            If ha_temp = 0 And hb_temp = 0 Then dr("angle_B") = 0 '左右都无水，则赋0，免得出现和倾角相同的的正值或负值。虽然由于A2=0，不影响渗透力计算结果，但是容易引起误解
            If dr("angle_B") < 0 Then dr("angle_B") = 0 '避免负值

            If dr("angle_B") = 0 Then '注意，此处0值可能是公式计算得来，也可能是左右都无水，则赋0
                dr("angle_B") = DBNull.Value
            End If



            '计算面积
            Dim uh12, lh12 As Double
            uh12 = dtt.Rows(k)("y2") - dtt.Rows(k)("y1")
            lh12 = dtt.Rows(k + 1)("y2") - dtt.Rows(k + 1)("y1")
            Dim A As Double = (uh12 + lh12) * dx * 0.5
            dr("A") = A

            Dim uhw1, lhw1 As Double
            uhw1 = dtt.Rows(k)("yw") - dtt.Rows(k)("y1")
            lhw1 = dtt.Rows(k + 1)("yw") - dtt.Rows(k + 1)("y1")
            Dim A_temp As Double = (uhw1 + lhw1) * dx * 0.5

            'If A_temp <= A Then
            '    dr("A3") = A_temp
            '    'dr("A4")=0
            '    dr("A1") = A - A_temp
            'Else
            '    dr("A4") = A_temp - A
            '    dr("A3") = A
            '    'dr("A1") = 0 '全被水淹了，无水上面积
            'End If

            If A_temp <= A Then
                dr("A2") = A_temp '水位以下面积
                dr("A1") = A - A_temp '水位以上面积
            Else
                'dr("A4") = A_temp - A
                dr("A2") = A '水位以下面积

                'dr("A1") = 0 '全被水淹了，无水上面积，不再输入数据
            End If

            dr("r1") = 20
            dr("r2") = 21
            dr("c") = 15
            dr("fai") = 17
            dr("safe_factor") = 1.15
            If String.IsNullOrEmpty(dtt.Rows(k)("ID").ToString) = False Then
                If dtt.Rows(k)("ID").ToString.Contains("*") = True Then
                    dr("ID") = k & "*"
                Else
                    dr("ID") = k
                End If
            Else
                dr("ID") = k
            End If
            dt_hp_temp.Rows.Add(dr)

        Next


        '以下是格式整理’
        For j = 0 To dt_hp_temp.Rows.Count - 1
            If dt_hp_temp.Rows(j).IsNull("A") = False Then
                dt_hp_temp.Rows(j)("A") = Math.Round(dt_hp_temp.Rows(j)("A"), 2)
            End If
            If dt_hp_temp.Rows(j).IsNull("A1") = False Then
                dt_hp_temp.Rows(j)("A1") = Math.Round(dt_hp_temp.Rows(j)("A1"), 2)
            End If
            If dt_hp_temp.Rows(j).IsNull("A2") = False Then
                dt_hp_temp.Rows(j)("A2") = Math.Round(dt_hp_temp.Rows(j)("A2"), 2)
            End If
            'If dt_hp_temp.Rows(j).IsNull("A3") = False Then
            '    dt_hp_temp.Rows(j)("A3") = Math.Round(dt_hp_temp.Rows(j)("A3"), 2)
            'End If
            'If dt_hp_temp.Rows(j).IsNull("A4") = False Then
            '    dt_hp_temp.Rows(j)("A4") = Math.Round(dt_hp_temp.Rows(j)("A4"), 2)
            'End If

            dt_hp_temp.Rows(j)("angle") = Math.Round(dt_hp_temp.Rows(j)("angle"), 2)
            dt_hp_temp.Rows(j)("L") = Math.Round(dt_hp_temp.Rows(j)("L"), 2)

            If dt_hp_temp.Rows(j).IsNull("angle_B") = False Then
                dt_hp_temp.Rows(j)("angle_B") = Math.Round(dt_hp_temp.Rows(j)("angle_B"), 2)
            End If

            If dt_hp_temp.Rows(j).IsNull("Ha") = False Then
                dt_hp_temp.Rows(j)("Ha") = Math.Round(dt_hp_temp.Rows(j)("Ha"), 2)
            End If

            If dt_hp_temp.Rows(j).IsNull("Hb") = False Then
                dt_hp_temp.Rows(j)("Hb") = Math.Round(dt_hp_temp.Rows(j)("Hb"), 2)
            End If
            '删除0值'
            If dt_hp_temp.Rows(j).IsNull("A") = False Then
                If dt_hp_temp.Rows(j)("A") = 0 Then
                    dt_hp_temp.Rows(j)("A") = DBNull.Value
                End If
            End If

            If dt_hp_temp.Rows(j).IsNull("A1") = False Then
                If dt_hp_temp.Rows(j)("A1") = 0 Then
                    dt_hp_temp.Rows(j)("A1") = DBNull.Value
                End If
            End If

            If dt_hp_temp.Rows(j).IsNull("A2") = False Then
                If dt_hp_temp.Rows(j)("A2") = 0 Then
                    dt_hp_temp.Rows(j)("A2") = DBNull.Value
                End If
            End If




            If dt_hp_temp.Rows(j).IsNull("Ha") = False Then
                If dt_hp_temp.Rows(j)("Ha") = 0 Then
                    dt_hp_temp.Rows(j)("Ha") = DBNull.Value
                End If
            End If

            If dt_hp_temp.Rows(j).IsNull("Hb") = False Then
                If dt_hp_temp.Rows(j)("Hb") = 0 Then
                    dt_hp_temp.Rows(j)("Hb") = DBNull.Value
                End If
            End If
        Next

        Return dt_hp_temp
    End Function
End Module
