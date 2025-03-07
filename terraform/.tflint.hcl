plugin "terraform" {
    enabled = true
    preset  = "recommended"
}

rule "terraform_unused_declarations" {
    enabled = true
}

rule "terraform_required_providers" {
    enabled = true
}
