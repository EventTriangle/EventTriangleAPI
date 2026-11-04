# Archived platform installers

These files are historical references. The maintained dev platform configuration
is in the FluxCD repository. See the complete
[platform migration map](../../sprint-planning/platform-migration.md) before
installing, upgrading or removing anything from this archive.

Do not run archived installers alongside Flux management. In particular,
`rabbitmq/Uninstall-RabbitMq.ps1` deletes persistent volume claims; it is not a
migration or rollback procedure. Archived credentials are not inputs to the new
configuration. Use the protected Secret workflow described in the migration guide.
