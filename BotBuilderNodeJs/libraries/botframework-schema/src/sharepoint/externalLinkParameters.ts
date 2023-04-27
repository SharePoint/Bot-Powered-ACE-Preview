// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint ExternalLinkParameters object
 */
export class ExternalLinkParameters {
    private isTeamsDeepLink: boolean;
    private target: string;
    
    /**
     * Initializes a new instance of the ExternalLinkParameters class
     */
    public ExternalLinkParameters() {
        // Do nothing
    }

    /**
     * Sets target property of type string
     */
    public set Target(target: string) {
        this.target = target;
    }

    /**
     * Gets target property of type string
     */
    public get Target(): string {
        return this.target;
    }

    /**
     * Sets isTeamsDeepLink property of type boolean
     */
    public set IsTeamsDeepLink (isTeamsDeepLink: boolean) {
        this.isTeamsDeepLink = isTeamsDeepLink;
    }

    /**
     * Gets isTeamsDeepLink property of type boolean
     */
    public get IsTeamsDeepLink(): boolean {
        return this.isTeamsDeepLink; 
    }
}