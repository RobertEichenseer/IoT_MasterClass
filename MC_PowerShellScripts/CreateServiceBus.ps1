param
(
        [bool]$FalseValue = $false
)

#Get-AzureSubscription
$SubscriptionName = "Windows Azure Internal Consumption"
$ServiceBusNamespaceName = "IoTMC-ns"
$EventHubName = "IoTMasterClass"

Add-AzureAccount
Select-AzureSubscription $SubscriptionName
New-AzureSBNamespace -Name $ServiceBusNamespaceName -Location "West Europe" -CreateACSNamespace $FalseValue -NamespaceType Messaging
$SBNamespace = Get-AzureSBNamespace -Name "IoTMC-ns"
Write-Output $SBNamespace.ConnectionString
