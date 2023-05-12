// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint ExternalLinkActionParameters object
 */
export class ExternalLinkActionParameters {
    private isTeamsDeepLink: boolean;
    private target: string;
    
    /**
     * Initializes a new instance of the ExternalLinkActionParameters class
     */
    public ExternalLinkActionParameters() {
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