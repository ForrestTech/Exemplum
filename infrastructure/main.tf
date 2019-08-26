provider "azurerm" {
    version = "=1.20.0"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "TerraformState"
    storage_account_name = "terrafstates"
    container_name       = "environmentstates"
    key = "exemplum"
  }
} 

resource "azurerm_resource_group" "rg" {
    name     = "${var.prefix}-${var.env}-${var.loc}-rg"
    location = "${var.location}"

     tags {
        environment = "${var.env}"
    }
}

resource "azurerm_sql_server" "sqlserver" {
  name                         = "${var.prefix}-${var.env}-${var.loc}-sql"
  resource_group_name          = "${azurerm_resource_group.rg.name}"
  location                     = "${var.location}"
  version                      = "12.0"
  administrator_login          = "sqladmin"
  administrator_login_password = "${var.sqlAdminPassword}"
}

resource "azurerm_sql_database" "qanda-db" {
  name                = "${var.prefix}-${var.env}-${var.loc}-qanda-db"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  location            = "${var.location}"
  server_name         = "${azurerm_sql_server.sqlserver.name}"

  tags = {
    environment = "${var.env}"
  }
}

resource "azurerm_sql_firewall_rule" "sql-firewall" {
  name                = "${var.prefix}-${var.env}-${var.loc}-sql-firewall"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.sqlserver.name}"
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_application_insights" "app-insights" {
  name                = "${var.prefix}-${var.env}-${var.loc}-app-insights"
  location            = "${var.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  application_type    = "web"
}

resource "azurerm_app_service_plan" "qanda-sp" {
  name                = "${var.prefix}-${var.env}-${var.loc}-qanda-sp"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_app_service" "qanda-as" {
  name                = "${var.prefix}-${var.env}-${var.loc}-qanda-as"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  app_service_plan_id = "${azurerm_app_service_plan.qanda-sp.id}"

  site_config {
    dotnet_framework_version = "v4.0"    
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.app-insights.instrumentation_key}"
  }

  connection_string {
    name  = "QandADatabase"
    type  = "SQLServer"
    value = "Server=tcp:${azurerm_sql_server.sqlserver.name}.database.windows.net,1433;Initial Catalog=${azurerm_sql_database.qanda-db.name};Persist Security Info=False;User ID=${azurerm_sql_server.sqlserver.administrator_login};Password=${azurerm_sql_server.sqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}

output "instrumentation_key" {
  value = "${azurerm_application_insights.app-insights.instrumentation_key}"
}