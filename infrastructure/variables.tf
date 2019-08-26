variable "location" {
     description = "The azure region to deploy the application to"
}

variable "loc" {
    description = "A short name for the azure region used when naming resources"
}

variable "env" {
    description = "A short name for the environment e.g dev, qa, prod"
}

variable "sqlAdminPassword" {
    description = "The admin password for sql databse"
}

variable "prefix" {
    type = "string"
    description = "The prefix used for all resources in this example"
    default = "exemplum"
}