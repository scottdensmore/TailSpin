. ".\PS-Scripts\Common-checks.ps1"

$res = $FALSE;

# Checks for any version of VS 2010 in x86
$res = $res -or (Check-RegistryPath -Path 'HKLM:SOFTWARE\Microsoft\VisualStudio\10.0\Setup\VS\BuildNumber')

# Checks for any version of VS 2010 in x64
$res = $res -or (Check-RegistryPath -Path 'HKLM:SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\Setup\VS\BuildNumber')

$res
# SIG # Begin signature block
# MIIbXgYJKoZIhvcNAQcCoIIbTzCCG0sCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU/OjiE4sfs+XbZBwB6TK9k9+c
# tlGgghXnMIIEhTCCA22gAwIBAgIKYQh3XwAAAAAASjANBgkqhkiG9w0BAQUFADB5
# MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVk
# bW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSMwIQYDVQQDExpN
# aWNyb3NvZnQgQ29kZSBTaWduaW5nIFBDQTAeFw0xMDA3MTkyMjUzMTBaFw0xMTEw
# MTkyMjUzMTBaMIGDMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQ
# MA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
# MQ0wCwYDVQQLEwRNT1BSMR4wHAYDVQQDExVNaWNyb3NvZnQgQ29ycG9yYXRpb24w
# ggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDMfoB7QfqLtE2ZKUPd5vhS
# 31tCl70zGZzvtdrQwNzo8be+XOEKssI1oH7k89Q4p/IPQnKnupvPHyTeVg2Ec8zT
# Y/cXza9pb2FNTIBPyHccUWTI0wTzFCazuEI/M0Ow2yZZWJO1bYliDwdWW9USoLEJ
# zBYr+YzfYVboBnh/13OIJgNn0aP/NIKsLUzYAuztb7miMcUyTLv2/V9iW17x9mSR
# x4V3Rxz2pytHLQs3VE+2HwSNtexgJ6fzfrZ8oUyRpotx1sY1YUe8wRVL3awzIr4G
# GZZLih5uAOu+HmndWFkhAys8vuYTBRdmNaeGLRrx2TTAAYAa/JpGnW6k50jcNeo3
# AgMBAAGjggECMIH/MBMGA1UdJQQMMAoGCCsGAQUFBwMDMB0GA1UdDgQWBBTkb1/K
# iflTvvBwVwZQqsJ5DaxWmzAOBgNVHQ8BAf8EBAMCB4AwHwYDVR0jBBgwFoAUV0V0
# HF2w9shDBeCMVC2PMqf+SJYwSQYDVR0fBEIwQDA+oDygOoY4aHR0cDovL2NybC5t
# aWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVjdHMvQ29kZVNpZ1BDQS5jcmwwTQYI
# KwYBBQUHAQEEQTA/MD0GCCsGAQUFBzAChjFodHRwOi8vd3d3Lm1pY3Jvc29mdC5j
# b20vcGtpL2NlcnRzL0NvZGVTaWdQQ0EuY3J0MA0GCSqGSIb3DQEBBQUAA4IBAQCW
# QV09XCukzrgqRS9CAl6hkBxMdCcJzgtUfXAoQXI7pBW80QRSV693dG0LVVeHNxBJ
# Htbm1MJjlLT9pC4t+l/iGVkVhNm3tulSolLeI18gL1nTqxqEfx3jveoP3wGIOSXY
# 7WRhGRt6r6YUSkZQSNlyS9VLkt3wD4R8y9PG8kx4bv28/2YQCvcDXRXqXpgMCdKZ
# jy61IL1h5slvBRQRy3v7Qgi9A/EmV2dNj1mLuETHRdNiBAwtMANYXuxI9G6oFDEE
# /TSiuAQtAt06oQlerWGxj3DAeFUjY6ifWXei27kbcSgc/C7hBI+17zS+SplH42RS
# Fjz6veDo4BQMECI0goIeMIIEyjCCA7KgAwIBAgIKYQPc9gAAAAAADDANBgkqhkiG
# 9w0BAQUFADB3MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSEw
# HwYDVQQDExhNaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EwHhcNMDgwNzI1MTkxMjUw
# WhcNMTEwNzI1MTkyMjUwWjCBszELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hp
# bmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jw
# b3JhdGlvbjENMAsGA1UECxMETU9QUjEnMCUGA1UECxMebkNpcGhlciBEU0UgRVNO
# OjE1OUMtQTNGNy0yNTcwMSUwIwYDVQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBT
# ZXJ2aWNlMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwO2BFKFed8Bb
# 9HaJYvqtfGgUtPe9NdgTeVoXytlsUUViJnovH9jqwW4BF/nDph9n21GwLN6KF+3/
# IK006pj7pdYq0vFEJwdaLTqT/1ZTsMj18wPyScwW0PUATFj4m/UHJbFmF8C9yNJS
# hY3CKziywza++Yfa9I5dQ9cGv5kFn6TO/quNYWPnOcXzGPbY/DE2aXJaohpMPuqH
# JUKd0T7xl9IYMpNwVVOBHuM7Dei+gnht5vrNmKRv2+5m9JXIzTXJnrs2DYOWlCan
# kOCpNDvVwJ4+8NRHjYYMgqRYMDocduOtlWa0t/0JigVgD6MP4pOWWCKcnSvbopQY
# kJUCvQZAlQIDAQABo4IBGTCCARUwHQYDVR0OBBYEFNLtDR4kuzep2CBqTR3SFtUu
# vp7rMB8GA1UdIwQYMBaAFCM0+NlSRnAK7UD7dvuzK7DDNbMPMFQGA1UdHwRNMEsw
# SaBHoEWGQ2h0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3Rz
# L01pY3Jvc29mdFRpbWVTdGFtcFBDQS5jcmwwWAYIKwYBBQUHAQEETDBKMEgGCCsG
# AQUFBzAChjxodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jv
# c29mdFRpbWVTdGFtcFBDQS5jcnQwEwYDVR0lBAwwCgYIKwYBBQUHAwgwDgYDVR0P
# AQH/BAQDAgbAMA0GCSqGSIb3DQEBBQUAA4IBAQCcClXIzEQTNAzYYyd2fT76ODKD
# U53yCPky9cVucKHJsWNrGZ0JZ9md64pq22Bm6elSJvM7xmrTwlK+qLnraqp4jMkW
# fZCVoMwhs56Bvc3Biym9YiXvCVfnhk4q7IDKu/whFsQ/TlIZ5g6x2MHCeZBktFBz
# EDVeXRHBuLqqz1L2gJEA5u9RQ0bp0OiU9iwkDYrGsjGKo342bKQFTGcHKru7EKWl
# MBpy0AYgOySTWxXZOZPTcy0axNRsHqEI7PYxuGtL7O5cMwIUMox8ESAvIAN/+Qyd
# uNOeX9YI/IGgmbi7VW7NQks6TYwUK8rIEtNibuoNCp0Jo2bZeU+OGqL/zJgEMIIG
# BzCCA++gAwIBAgIKYRZoNAAAAAAAHDANBgkqhkiG9w0BAQUFADBfMRMwEQYKCZIm
# iZPyLGQBGRYDY29tMRkwFwYKCZImiZPyLGQBGRYJbWljcm9zb2Z0MS0wKwYDVQQD
# EyRNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkwHhcNMDcwNDAz
# MTI1MzA5WhcNMjEwNDAzMTMwMzA5WjB3MQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMSEwHwYDVQQDExhNaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Ew
# ggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCfoWyx39tIkip8ay4Z4b3i
# 48WZUSNQrc7dGE4kD+7Rp9FMrXQwIBHrB9VUlRVJlBtCkq6YXDAm2gBr6Hu97IkH
# D/cOBJjwicwfyzMkh53y9GccLPx754gd6udOo6HBI1PKjfpFzwnQXq/QsEIEovmm
# bJNn1yjcRlOwhtDlKEYuJ6yGT1VSDOQDLPtqkJAwbofzWTCd+n7Wl7PoIZd++NIT
# 8wi3U21StEWQn0gASkdmEScpZqiX5NMGgUqi+YSnEUcUCYKfhO1VeP4Bmh1QCIUA
# EDBG7bfeI0a7xC1Un68eeEExd8yb3zuDk6FhArUdDbH895uyAc4iS1T/+QXDwiAL
# AgMBAAGjggGrMIIBpzAPBgNVHRMBAf8EBTADAQH/MB0GA1UdDgQWBBQjNPjZUkZw
# Cu1A+3b7syuwwzWzDzALBgNVHQ8EBAMCAYYwEAYJKwYBBAGCNxUBBAMCAQAwgZgG
# A1UdIwSBkDCBjYAUDqyCYEBWJ5flJRP8KuEKU5VZ5KShY6RhMF8xEzARBgoJkiaJ
# k/IsZAEZFgNjb20xGTAXBgoJkiaJk/IsZAEZFgltaWNyb3NvZnQxLTArBgNVBAMT
# JE1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhvcml0eYIQea0WoUqgpa1M
# c1j0BxMuZTBQBgNVHR8ESTBHMEWgQ6BBhj9odHRwOi8vY3JsLm1pY3Jvc29mdC5j
# b20vcGtpL2NybC9wcm9kdWN0cy9taWNyb3NvZnRyb290Y2VydC5jcmwwVAYIKwYB
# BQUHAQEESDBGMEQGCCsGAQUFBzAChjhodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20v
# cGtpL2NlcnRzL01pY3Jvc29mdFJvb3RDZXJ0LmNydDATBgNVHSUEDDAKBggrBgEF
# BQcDCDANBgkqhkiG9w0BAQUFAAOCAgEAEJeKw1wDRDbd6bStd9vOeVFNAbEudHFb
# bQwTq86+e4+4LtQSooxtYrhXAstOIBNQmd16QOJXu69YmhzhHQGGrLt48ovQ7DsB
# 7uK+jwoFyI1I4vBTFd1Pq5Lk541q1YDB5pTyBi+FA+mRKiQicPv2/OR4mS4N9wfi
# cLwYTp2OawpylbihOZxnLcVRDupiXD8WmIsgP+IHGjL5zDFKdjE9K3ILyOpwPf+F
# ChPfwgphjvDXuBfrTot/xTUrXqO/67x9C0J71FNyIe4wyrt4ZVxbARcKFA7S2hSY
# 9Ty5ZlizLS/n+YWGzFFW6J1wlGysOUzU9nm/qhh6YinvopspNAZ3GmLJPR5tH4Lw
# C8csu89Ds+X57H2146SodDW4TsVxIxImdgs8UoxxWkZDFLyzs7BNZ8ifQv+AeSGA
# nhUwZuhCEl4ayJ4iIdBD6Svpu/RIzCzU2DKATCYqSCRfWupW76bemZ3KOm+9gSd0
# BhHudiG/m4LBJ1S2sWo9iaF2YbRuoROmv6pH8BJv/YoybLL+31HIjCPJZr2dHYcS
# ZAI9La9Zj7jkIeW1sMpjtHhUBdRBLlCslLCleKuzoJZ1GtmShxN1Ii8yqAhuoFuM
# Jb+g74TKIdbrHk/Jmu5J4PcBZW+JC33Iacjmbuqnl84xKf8OxVtc2E0bodj6L54/
# LlUWa8kTo/0wggaBMIIEaaADAgECAgphFQgnAAAAAAAMMA0GCSqGSIb3DQEBBQUA
# MF8xEzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJk/IsZAEZFgltaWNyb3Nv
# ZnQxLTArBgNVBAMTJE1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhvcml0
# eTAeFw0wNjAxMjUyMzIyMzJaFw0xNzAxMjUyMzMyMzJaMHkxCzAJBgNVBAYTAlVT
# MRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
# ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xIzAhBgNVBAMTGk1pY3Jvc29mdCBDb2Rl
# IFNpZ25pbmcgUENBMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAn43f
# hTeMsQZWZjZO1ArrNiORHq+rjVjpxM/BnzoKJMTExF6w7hUUxfo+mTNrGWly9HwF
# X+WZJUTXNRmKkNwojpAM79WQYa3e3BhwLYPJb6+FLPjdubkw/XF4HIP9yKm5gmcN
# erjBCcK8FpdXPxyY02nXMJCQkI0wH9gm1J57iNniCe2XSUXrBFKBdXu4tSK4Lla7
# 18+pTjwKg6KoOsWttgEOas8itCMfbNUn57d+wbTVMq15JRxChuKdhfRX2htZLy0m
# kinFs9eFo55gWpTme5x7XoI0S23/1O4n0KLc0ZAMzn0OFXyIrDTHwGyYhErJRHlo
# KN8igw24iixIYeL+EQIDAQABo4ICIzCCAh8wEAYJKwYBBAGCNxUBBAMCAQAwHQYD
# VR0OBBYEFFdFdBxdsPbIQwXgjFQtjzKn/kiWMAsGA1UdDwQEAwIBxjAPBgNVHRMB
# Af8EBTADAQH/MIGYBgNVHSMEgZAwgY2AFA6sgmBAVieX5SUT/CrhClOVWeSkoWOk
# YTBfMRMwEQYKCZImiZPyLGQBGRYDY29tMRkwFwYKCZImiZPyLGQBGRYJbWljcm9z
# b2Z0MS0wKwYDVQQDEyRNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0ZSBBdXRob3Jp
# dHmCEHmtFqFKoKWtTHNY9AcTLmUwUAYDVR0fBEkwRzBFoEOgQYY/aHR0cDovL2Ny
# bC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVjdHMvbWljcm9zb2Z0cm9vdGNl
# cnQuY3JsMFQGCCsGAQUFBwEBBEgwRjBEBggrBgEFBQcwAoY4aHR0cDovL3d3dy5t
# aWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNyb3NvZnRSb290Q2VydC5jcnQwdgYD
# VR0gBG8wbTBrBgkrBgEEAYI3FS8wXjBcBggrBgEFBQcCAjBQHk4AQwBvAHAAeQBy
# AGkAZwBoAHQAIACpACAAMgAwADAANgAgAE0AaQBjAHIAbwBzAG8AZgB0ACAAQwBv
# AHIAcABvAHIAYQB0AGkAbwBuAC4wEwYDVR0lBAwwCgYIKwYBBQUHAwMwDQYJKoZI
# hvcNAQEFBQADggIBADC8sCCkYqCn7zkmYT3crMaZ0IbELvWDMmVeIj6b1ob46Laf
# yovWO3ULoZE+TN1kdIxJ8oiMGGds/hVmRrg6RkKXyJE31CSx56zT6kEUg3fTyU8F
# X6MUUr+WpC8+VlsQdc5Tw84FVGm0ZckkpQ/hJbgauU3lArlQHk+zmAwdlQLuIlmt
# IssFdAsERXsEWeDYD7PrTPhg3cJ4ntG6n2v38+5+RBFA0r26m0sWCG6kvlXkpjgS
# o0j0HFV6iiDRff6R25SPL8J7a6ZkhU+j5Sw0KV0Lv/XHOC/EIMRWMfZpzoX4CpHs
# 0NauujgFDOtuT0ycAymqovwYoCkMDVxcViNX2hyWDcgmNsFEy+Xh5m+J54/pmLVz
# 03jj7aMBPHTlXrxs9iGJZwXsl521sf2vpulypcM04S+f+fRqOeItBIJb/NCcrnyd
# EfnmtVMZdLo5SjnrfUKzSjs3PcJKeyeY5+JOmxtKVDhqIze+ardI7upCDUkkkY63
# BC6Xb+TnRbuPTf1g2ddZwtiA1mA0e7ehkyD+gbiqpVwJ6YoNvihNftfoD+1leNEx
# X7lm299C5wvMAgeN3/8gBqNFZbSzMo0ukeJNtKnJ+rxrBA6yn+qf3qTJCpb0jffY
# mKjwhQIIWaQgpiwLGvJSBu1p5WQYG+Cjq97KfBRhQ7hl9TajVRMrZyxNGzBMMYIE
# 4TCCBN0CAQEwgYcweTELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24x
# EDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
# bjEjMCEGA1UEAxMaTWljcm9zb2Z0IENvZGUgU2lnbmluZyBQQ0ECCmEId18AAAAA
# AEowCQYFKw4DAhoFAKCCAQ0wGQYJKoZIhvcNAQkDMQwGCisGAQQBgjcCAQQwHAYK
# KwYBBAGCNwIBCzEOMAwGCisGAQQBgjcCARUwIwYJKoZIhvcNAQkEMRYEFJPNLa+L
# CipatUYB9cZTuMxgxmSiMIGsBgorBgEEAYI3AgEMMYGdMIGaoHCAbgBwAGEAdAB0
# AGUAcgBuAHMAIAAmACAAcAByAGEAYwB0AGkAYwBlAHMAIABXAGkAbgBkAG8AdwBz
# ACAAUABoAG8AbgBlACAANwAgAEQAZQB2AGUAbABvAHAAZQByACAARwB1AGkAZABh
# AG4AYwBloSaAJGh0dHA6Ly9tc2RuLm1pY3Jvc29mdC5jb20vcHJhY3RpY2VzIDAN
# BgkqhkiG9w0BAQEFAASCAQCEv66EecW5PvPo8qhmThzMtwfhYhDH0/GKv7w3tEQ8
# e5saEhiZQqV+otoWY9q7pak1pqaanTCtPWswvCbn9kbvDotI52MMOYIH8ywB3Mh6
# WCRQ25/dBLNQpyciaqAV4b2gILtChCH18unVThrVTU00HSen/2gV0CSkzU1pMora
# jEtgBNM9pvbi+JOiOpLzLrEd2sQhzS/KSLvOQcUCQfGgjHRbzTRLRGpPC+Otoi0O
# 603o8elOL2dw6Q1537hQwWmdDn8X2mG3NNG1XSYR515y2CCDbMKPPGK/dZKTYaBn
# MHkevKLX36F0MeQy/TU0P0aH4oCVQxWR6S9zl4+JzPhVoYICHTCCAhkGCSqGSIb3
# DQEJBjGCAgowggIGAgEBMIGFMHcxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
# aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
# cG9yYXRpb24xITAfBgNVBAMTGE1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQQIKYQPc
# 9gAAAAAADDAHBgUrDgMCGqBdMBgGCSqGSIb3DQEJAzELBgkqhkiG9w0BBwEwHAYJ
# KoZIhvcNAQkFMQ8XDTEwMTAyNzE5NTkxNFowIwYJKoZIhvcNAQkEMRYEFNGhR4s0
# uyLGOpKM+h/eKyuOcxeKMA0GCSqGSIb3DQEBBQUABIIBAEt1w4ovgKyySzMdpaAd
# 4Ma3n3dLXxAPW4Tqb4yxAxeVe7YGpwKZcR+1a348JouKsKv8lU6hmxCXMc9j4wSh
# mSkLxHpwbivE9qD5ifAIidc5v3pJ0r1kFUx8IGvHEUexb2k8Rw4r2POg8JANMeVY
# TyXh05beiwc+HhUFLDY25+Qjd0BppnUvZw+dtOJtDRZBJK0bs2XpoclCh3Uo6uWg
# xUSWbGC1y2w8g1J3/eNkYJpkeZUdMUdWVO634yD8yXGSnbLm37hAEafVg5iSRGaw
# 51EWiMrVddcMxigjHNX4Ubdrr2coyOb17UBvmRb3wRRlVy7xzSmEFjitcI+jUcUJ
# qa4=
# SIG # End signature block
