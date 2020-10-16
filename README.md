## General info
SteamGridDB Metadata is a provider for Playnite that leverages the [SteamGridDB Api](https://www.steamgriddb.com/api/v2) to curate your artwork.
	
## Installation
To use this provider, simply download the latest release and drag and drop the .pext file over-top of the Playnite window.

## Configuration
To use this provider, you *must* have a SteamGridDB Api key.  You can obtain this key by creating an account at [SteamGridDB](https://www.steamgriddb.com) and copy and requesting it on your [preferences page](https://www.steamgriddb.com/profile/preferences).

Enter the api in Playnite under Settings -> Metadata Sources -> SteamGrid DB api Key.

Options are fairly limited thus far but support is provided for cover style and cover size.  

Metadata can be fetched by editing a single game and downloading metadata of your choice from SteamGridDB or automatically scraping from a selection of games.  Currently, the provider will pick the top result from the api.