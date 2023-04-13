/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

'use strict';

const models = require('./index');

/**
 * Sub-resource.
 *
 * @extends models['BaseResource']
 */
class SubResource extends models['BaseResource'] {
  /**
   * Create a SubResource.
   * @member {string} [id] Resource ID
   */
  constructor() {
    super();
  }

  /**
   * Defines the metadata of SubResource
   *
   * @returns {object} metadata of SubResource
   *
   */
  mapper() {
    return {
      required: false,
      serializedName: 'SubResource',
      type: {
        name: 'Composite',
        className: 'SubResource',
        modelProperties: {
          id: {
            required: false,
            serializedName: 'id',
            type: {
              name: 'String'
            }
          }
        }
      }
    };
  }
}

module.exports = SubResource;
