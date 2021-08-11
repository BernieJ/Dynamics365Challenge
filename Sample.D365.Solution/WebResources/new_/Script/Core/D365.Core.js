if (typeof D365 === "undefined") { var D365 = {}; }
if (D365.Core === undefined) {
  D365.Core = {
    Form: {
      /**
      * @param {string} id The id
      * @param {string} entityType The entity type (eg. account)
      * @param {string} name The name
      * @returns {object} The lookup item
      */
      ToLookupItem: function (id, entityType, name) {
        return {
          id: D365.Core.Guid.ToString(id, "B"),
          entityType: entityType,
          name: name
        };
      },

      /**
      * Reset the options of an option set control to the supplied options
      * @param {object} control The control
      * @param {object[]} options The options
      */
      ResetOptionSetOptions: function (control, options) {
        var currentValue = control.getAttribute().getValue();
        var allOptions = control.getAttribute().getOptions(); // Note: Do NOT use control.getOptions - does not work in mobile app

        for (var i = 0; i < allOptions.length; i++) {
          if (allOptions[i].value !== currentValue) { control.removeOption(allOptions[i].value); }
        }

        for (var i = 0; i < options.length; i++) {
          if (options[i].value !== currentValue) { control.addOption(options[i]); }
        }
      },

      /** @enum {number} */
      FormTypes: {
        Undefined: 0,
        Create: 1,
        Update: 2,
        ReadOnly: 3,
        Disabled: 4,
        BulkEdit: 6
      },

      /** @enum {string} */
      RequiredLevel: {
        None: "none",
        Required: "required",
        Recommended: "recommended"
      },

      /** @enum {string} */
      SubmitMode: {
        Always: "always",
        Never: "never",
        Dirty: "dirty"
      },

      /** @enum {number} */
      SaveMode: {
        Save: 1,
        SaveAndClose: 2,
        Deactivate: 5,
        Reactivate: 6,
        Send: 7,
        Disqualify: 15,
        Qualify: 16,
        Assign: 47,
        SaveAsCompleted: 58,
        SaveAndNew: 59,
        AutoSave: 70
      }
    },

    /** @enum {string} */
    WebApi: {
      /** @enum {number} */
      ConditionOperators: {
        Equal: "eq",
        NotEqual: "ne",
        GreaterThan: "gt",
        GreaterOrEqual: "qe",
        LessThan: "lt",
        LessOrEqual: "le",
      },

      /** @enum {string} */
      LogicalOperators: {
        And: "and",
        Or: "or",
        Not: "not",
      },

      /**
      * @callback retrieveSuccessCallback
      * @param {object} entity The entity retrieved
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * Retrieve an entity from the Web API
      * @param {object} context The context
      * @param {string} entitySet The entity set
      * @param {string} id The id of the entity
      * @param {string[]} [select] The list of fields to select
      * @param {retrieveSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Retrieve: function (context, entitySet, id, select, successCallback, failCallback, additionalQueryItems, async) {
        var path = [entitySet, "(", D365.Core.Guid.ToString(id), ")"];

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        return D365.Core.WebApi.Get(
          context,
          path.join(""),
          function (xhr) {
            if (successCallback) {
              var entity = JSON.parse(xhr.responseText);
              return successCallback(entity, xhr);
            }
          },
          failCallback,
          async);
      },

      /**
      * @callback retrieveMultipleSuccessCallback
      * @param {object[]} entities The entity retrieved
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * Retrieve a list of entities from the Web API
      * @param {object} context The context
      * @param {string} entitySet The entity set
      * @param {string[]} [select] The list of fields to select
      * @param {string} [filter] The filter
      * @param {number} [top] The number of entities to retrieve
      * @param {retrieveMultipleSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveMultiple: function (context, entitySet, select, filter, top, successCallback, failCallback, additionalQueryItems, async) {
        var path = [entitySet];

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (filter) { query.push(["$filter=", filter].join("")); }
        if (top) { query.push(["$top=", top].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        return D365.Core.WebApi.Get(
          context,
          path.join(""),
          function (xhr) {
            if (successCallback) {
              var entities = JSON.parse(xhr.responseText).value;
              return successCallback(entities, xhr);
            }
          },
          failCallback,
          async);
      },

      /**
      * @callback retrieveMultiplePagedSuccessCallback
      * @param {object} entities The list of entities
      */
      /**
      * @callback retrieveMultiplePagedFailCallback
      * @param {object} error The error object
      */
      /**
      * Retrieve a list of entities from the Web API
      * @param {object} context The context
      * @param {string} entitySet The entity set
      * @param {string[]} [select] The list of fields to select
      * @param {string} [filter] The filter
      * @param {number} [top] The number of entities to retrieve
      * @param {retrieveMultiplePagedSuccessCallback} [successCallback] The success callback
      * @param {retrieveMultiplePagedFailCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveMultiplePaged: function (context, entitySet, select, filter, top, successCallback, failCallback, additionalQueryItems, async) {
        var path = [entitySet];

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (filter) { query.push(["$filter=", filter].join("")); }
        if (top) { query.push(["$top=", top].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        var entities = [];

        function getPage(path) {
          return D365.Core.WebApi.Get(
            context,
            path,
            function (xhr) {
              if (successCallback) {
                var response = JSON.parse(xhr.responseText);
                entities = entities.concat(response.value);

                if (response['@odata.nextLink']) {
                  return getPage([entitySet, response['@odata.nextLink'].substring(response['@odata.nextLink'].indexOf('?'))].join(""));
                }
                else {
                  return successCallback(entities, xhr);
                }
              }
            },
            failCallback,
            async);
        }

        return getPage(path.join(""));
      },

      /**
      * Send a GET request to the Dynamics Web API
      * @param {object} context The context
      * @param {string} query The query
      * @param {successCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Get: function (context, query, successCallback, failCallback, async) {
        return D365.Core.WebApi.Request(
          context,
          "GET",
          query,
          null,
          {
            "OData-MaxVersion": "4.0",
            "OData-Version": "4.0",
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8",
            "Prefer": 'odata.include-annotations="*"'
          },
          successCallback,
          failCallback,
          async);
      },

      /**
      * Send a PATCH request to the Dynamics Web API
      * @param {object} context The context
      * @param {string} query The query
      * @param {string} body The body
      * @param {successCallback} [successCallback] The success callback
      * @param {failCallback}[failCallback]  The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Patch: function (context, query, body, successCallback, failCallback, async) {
        return D365.Core.WebApi.Request(
          context,
          "PATCH",
          query,
          body,
          {
            "OData-MaxVersion": "4.0",
            "OData-Version": "4.0",
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8",
            "Prefer": "return=representation"
          },
          successCallback,
          failCallback,
          async);
      },

      /**
      * Send a POST request to the Dynamics Web API
      * @param {object} context The context
      * @param {string} entitySet The set of entity to create
      * @param {string} body The body
      * @param {successCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback]The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Post: function (context, entitySet, body, successCallback, failCallback, async) {
        return D365.Core.WebApi.Request(
          context,
          "POST",
          entitySet,
          body,
          {
            "OData-MaxVersion": "4.0",
            "OData-Version": "4.0",
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8",
            "Prefer": "return=representation"
          },
          successCallback,
          failCallback,
          async);
      },

      /**
      * Send a DELETE request to the Dynamics Web API
      * @param {object} context The context
      * @param {string} entitySet The set of entity to create
      * @param {string} id The id of the entity
      * @param {successCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback]The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Delete: function (context, entitySet, id, successCallback, failCallback, async) {
        var path = [entitySet, "(", D365.Core.Guid.ToString(id), ")"];

        return D365.Core.WebApi.Request(
          context,
          "DELETE",
          path.join(""),
          null,
          {
            "OData-MaxVersion": "4.0",
            "OData-Version": "4.0",
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8",
            "Prefer": "return=representation"
          },
          successCallback,
          failCallback,
          async);
      },

      /**
      * @callback successCallback
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * @callback failCallback
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * Send an HTTP request to the Dynamics Web API
      * @param {object} context The context
      * @param {string} method The method
      * @param {string} query The query
      * @param {string} body The body
      * @param {string[]} headers The headers
      * @param {successCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      Request: function (context, method, query, body, headers, successCallback, failCallback, async) {
        if (async === undefined) { async = true; }

        var xhr = new XMLHttpRequest();
        xhr.open(
          method,
          [context.getClientUrl(), "/api/data/v9.0/", query].join(""),
          async
        );

        for (var header in headers) {
          xhr.setRequestHeader(header, headers[header]);
        }

        if (async) {
          xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
              xhr.onreadystatechange = null;

              switch (xhr.status) {
                case 200:
                case 201:
                case 204:
                case 304:
                  if (successCallback) {
                    successCallback(xhr);
                  }
                  break;
                default:
                  if (failCallback) {
                    failCallback(xhr);
                  }
              }
            }
          };

          xhr.send(body);
        } else {
          try {
            xhr.send(body);
            switch (xhr.status) {
              case 200:
              case 201:
              case 204:
              case 304:
                if (successCallback) {
                  return successCallback(xhr);
                }
                break;
              default:
                if (failCallback) {
                  return failCallback(xhr);
                }
            }
          }
          catch (error) {
            if (failCallback) {
              return failCallback(xhr);
            }
          }
        }
      }
    },

    MetaData: {
      AttributeTypes: {
        BigIntAttributeMetadata: "BigIntAttributeMetadata",
        BooleanOptionSetMetadata: "BooleanOptionSetMetadata",
        DateTimeAttributeMetadata: "DateTimeAttributeMetadata",
        DecimalAttributeMetadata: "DecimalAttributeMetadata",
        DoubleAttributeMetadata: "DoubleAttributeMetadata",
        EnumAttributeMetadata: "EnumAttributeMetadata",
        FileAttributeMetadata: "FileAttributeMetadata",
        ImageAttributeMetadata: "ImageAttributeMetadata",
        IntegerAttributeMetadata: "IntegerAttributeMetadata",
        LookupAttributeMetadata: "LookupAttributeMetadata",
        ManagedPropertyAttributeMetadata: "ManagedPropertyAttributeMetadata",
        MemoAttributeMetadata: "MemoAttributeMetadata",
        MoneyAttributeMetadata: "MoneyAttributeMetadata",
        MultiSelectPicklistAttributeMetadata: "MultiSelectPicklistAttributeMetadata",
        PicklistAttributeMetadata: "PicklistAttributeMetadata",
        StringAttributeMetadata: "StringAttributeMetadata",
        UniqueIdentifierAttributeMetadata: "UniqueIdentifierAttributeMetadata",
      },

      /**
      * @callback retrieveMetaDataSuccessCallback
      * @param {object} metadata The metadata retrieved
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * Retrieve all entity metadata
      * @param {object} context The context
      * @param {string[]} [select] The list of fields to select
      * @param {retrieveMetaDataSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveAllMetaData: function (context, select, successCallback, failCallback, additionalQueryItems, async) {
        var path = ["EntityDefinitions"];

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        return D365.Core.WebApi.Get(
          context,
          path.join(""),
          function (xhr) {
            if (successCallback) {
              var metadata = JSON.parse(xhr.responseText);
              return successCallback(metadata, xhr);
            }
          },
          failCallback,
          async);
      },

      /**
      * Retrieve the entity metadata 
      * @param {object} context The context
      * @param {string} entityLogicalName The entity logical name
      * @param {string[]} [select] The list of fields to select
      * @param {retrieveMetaDataSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveEntityMetaData: function (context, entityLogicalName, select, successCallback, failCallback, additionalQueryItems, async) {
        var path = ["EntityDefinitions", "(LogicalName='", entityLogicalName, "')"];

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        return D365.Core.WebApi.Get(
          context,
          path.join(""),
          function (xhr) {
            if (successCallback) {
              var metadata = JSON.parse(xhr.responseText);
              return successCallback(metadata, xhr);
            }
          },
          failCallback,
          async);
      },

      /**
      * Retrieve the attribute metadata
      * @param {object} context The context
      * @param {string} entityLogicalName The entity logical name
      * @param {string} attributeName The name of the attribute
      * @param {string} [attributeType] The metadata type of the attribute.
      * @param {string[]} [select] The list of fields to select
      * @param {retrieveMetaDataSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {string[]} [additionalQueryItems] Additional query options
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveAttributeMetaData: function (context, entityLogicalName, attributeName, attributeType, select, successCallback, failCallback, additionalQueryItems, async) {
        var path = ["EntityDefinitions", "(LogicalName='", entityLogicalName, "')/Attributes(LogicalName='", attributeName, "')"];

        if (attributeType) path.push(["/Microsoft.Dynamics.CRM.", attributeType].join(""));

        var query = [];
        if (select) { query.push(["$select=", select.join(",")].join("")); }
        if (additionalQueryItems) { query.push(additionalQueryItems); }

        if (query.length > 0) { path.push("?", query.join("&")); }

        return D365.Core.WebApi.Get(
          context,
          path.join(""),
          function (xhr) {
            if (successCallback) {
              var metadata = JSON.parse(xhr.responseText);
              return successCallback(metadata, xhr);
            }
          },
          failCallback,
          async);
      },

      /**
      * @callback retrieveOptionSetSuccessCallback
      * @param {object} optionSet The option set metadata retrieved
      * @param {XMLHttpRequest} xhr The xhr object
      */
      /**
      * Retrieve the option set metadata (including the options list)
      * @param {object} context The context
      * @param {string} entityLogicalName The entity logical name
      * @param {string} optionSetName The name of the option set
      * @param {retrieveOptionSetSuccessCallback} [successCallback] The success callback
      * @param {failCallback} [failCallback] The fail callback
      * @param {boolean} [async=true] A value indicating whether to run the request asynchronously or not. Defaults to true.
      * @returns {object} Asynchronous: void. Synchronous: the result of the executed callback.
      */
      RetrieveOptionSet: function (context, entityLogicalName, optionSetName, successCallback, failCallback, async) {
        return D365.Core.MetaData.RetrieveAttributeMetaData(
          context,
          entityLogicalName,
          optionSetName,
          D365.Core.MetaData.AttributeTypes.PicklistAttributeMetadata,
          null,
          successCallback,
          failCallback,
          "$expand=OptionSet,GlobalOptionSet",
          async
        );
      },
    },

    Security: {
      /**
      * @type {{ name: string, ownerid: string, teamid: string }[]}
      */
      SystemUserTeams: null,

      /**
      * @type {{ name: string, roleid: string }[]}
      */
      Roles: null,

      /**
      * Checks whether the supplied user is a member of any of the supplied teams
      * @param {object} context The context
      * @param {string} systemUserId The Id of the user
      * @param {...string} teamNames The names of the teams
      * @returns {boolean} A value indicating whether the user is a member of any of the supplied teams
      */
      SystemUserIsMemberOfTeam: function (context, systemUserId, teamNames) {
        if (D365.Core.Security.SystemUserTeams === null) {
          D365.Core.Security.SystemUserTeams = [];

          D365.Core.WebApi.Retrieve(
            context,
            "systemusers",
            systemUserId,
            ["systemuserid"],
            function (systemUser) {
              D365.Core.Security.SystemUserTeams = systemUser.teammembership_association;
            },
            function (xhr) { console.log(xhr); },
            ["$expand=teammembership_association($select=name)"],
            false);
        }

        for (var iTeam = 0; iTeam < D365.Core.Security.SystemUserTeams.length; iTeam++) {
          for (var iArgument = 2; iArgument < arguments.length; iArgument++) {
            if (D365.Core.Security.SystemUserTeams[iTeam].name === arguments[iArgument]) { return true; }
          }
        }

        return false;
      },

      /**
      * Checks whether the supplied user is a member of any of the supplied roles
      * @param {object} context The context
      * @param {...string} roleNames The names of the roles
      * @returns {boolean} A value indicating whether the user is a member of any of the supplied roles
      */
      SystemUserHasRole: function (context, roleNames) {
        if (D365.Core.Security.Roles === null) {
          D365.Core.Security.Roles = [];
          var userRoles = context.userSettings.securityRoles;

          D365.Core.WebApi.RetrieveMultiplePaged(
            context,
            "roles",
            ["roleid", "name"],
            null,
            null,
            function (roles) {
              D365.Core.Security.Roles = roles.filter(function (role) {
                for (var iUserRole = 0; iUserRole < userRoles.length; iUserRole++) {
                  if (role.roleid === userRoles[iUserRole]) {
                    return true;
                  }
                }
                return false;
              });
            },
            function (xhr) { console.log(xhr); },
            null,
            false);
        }

        for (var iRole = 0; iRole < D365.Core.Security.Roles.length; iRole++) {
          for (var iArgument = 1; iArgument < arguments.length; iArgument++) {
            if (D365.Core.Security.Roles[iRole].name === arguments[iArgument]) { return true; }
          }
        }

        return false;
      }
    },

    Date: {
      /**
      * Gets the current date and time
      * @returns {Date} The current date and time
      */
      Now: function () {
        return new Date();
      },

      /**
      * Gets the current date
      * @returns {Date} The current date
      */
      Today: function () {
        var now = D365.Core.Date.Now();
        return new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
      },

      /**
      * Adds the specified number of days to the supplied date
      * @param {Date} date The date
      * @param {number} days The number of days to add
      * @returns {Date} The date with the number of days added
      */
      AddDays: function (date, days) {
        var d = new Date(date);
        d.setDate(d.getDate() + days);
        return d;
      },

      /** @enum {number} */
      DayOfTheWeek: {
        Monday: 1,
        Tuesday: 2,
        Wednesday: 3,
        Thursday: 4,
        Friday: 5,
        Saturday: 6,
        Sunday: 7
      }
    },

    Guid: {
      /**
      * Checks whether the supplied Guids are equal
      * @param {string} guid1 The first Guid
      * @param {string} guid2 The second Guid
      * @returns {boolean} A value indicating whether the supplied Guids are equal
      */
      Equal: function (guid1, guid2) {
        return D365.Core.Guid.ToString(guid1) === D365.Core.Guid.ToString(guid2);
      },

      /**
      * Converts the supplied Guid to a string
      * @param {string} guid The Guid
      * @param {string} [format="D"] The format (N, B or D). Defaults to D.
      * @returns {string} The formatted Guid
      */
      ToString: function (guid, format) {
        format = format || "D";
        var v = (guid || "").replace(/[-{}]/g, "").toLowerCase();
        var a = [v.substr(0, 8), v.substr(8, 4), v.substr(12, 4), v.substr(16, 4), v.substr(20, 12)];

        switch (format.toUpperCase()) {
          case 'N': return a.join("");
          case 'B': return ["{", a.join("-"), "}"].join("");
          default: return a.join("-");
        }
      }
    },

    Utilities: {
      /**
      * Parse and return all the query string parameters from the url including the "Data=" parameters
      * @param {string} url The url to read parameters from.
      * @returns {object} An object of key-value pairs.
      */
      GetQueryStringParameters(url) {
        var vars = [], hash;
        var hashes = unescape(url.replace('?', ''));
        hashes = hashes.replace('Data=', '').replace('data=', '').split(',');
        for (var i = 0; i < hashes.length; i++) {
          hash = hashes[i].split('=');
          vars.push(hash[0]);
          vars[hash[0]] = hash[1];
        }
        return vars;
      },

      /**
     * Determine if the function is running in a UI or Classic D365 environment
     * @returns {object} a true or false value
     */
      IsUnifiedInterface: function () {
        try {
          if (Xrm && Xrm.Internal && Xrm.Internal.isUci) {
            return Xrm.Internal.isUci();
          }

          if (Xrm && Xrm.Utility && Xrm.Utility.getGlobalContext) {
            var globalContext = Xrm.Utility.getGlobalContext();
            return globalContext.getCurrentAppUrl() !== globalContext.getClientUrl();
          }

          return true;
        }
        catch {
          return true;
        }
      },

      /**
      * Get a specific query string value including parameters in the "Data=" parameter list
      * @param {string} url The url to read parameters from.
      * @param {string} parameter the name of the query string parameter to read.
      * @returns {object} A value for the query string parameter.
      */
      GetQueryStringParameter(url, parameter) {
        return GetQueryStringParameters(url)[parameter];
      },

      /**
      * @callback setWebResourceControlContextSuccessCallback
      * @param {object} contentWindow The window object of the iframe containing the webresource
      */
      /**
      * Sets the form context and Xrm object of the web resource
      * @param {object} formContext The form context to set.
      * @param {string} controlName The form context to set.
      * @param {setWebResourceControlContextSuccessCallback} [successCallback] The form context to set.
      * @param {function} [failedCallback] The form context to set.
      */
      SetWebResourceControlContext: function (formContext, controlName, successCallback, failedCallback) {
        var wrControl = formContext.getControl(controlName);
        if (wrControl) {
          if (wrControl.getContentWindow) {
            wrControl.getContentWindow().then(
              function (contentWindow) {
                if (contentWindow.setClientApiContext) {
                  contentWindow.setClientApiContext(Xrm, formContext);
                }
                successCallback(contentWindow);
              },
              failedCallback
            )
          }
          else {
            // fallback until classic mode is disabled for good
            try {
              var contentWindow = wrControl.getObject().contentWindow;

              if (contentWindow.setClientApiContext) {
                contentWindow.setClientApiContext(Xrm, formContext);
                if (successCallback) {
                  successCallback(contentWindow);
                }
              }
              else {
                var token = setInterval(function () {
                  var contentWindow = wrControl.getObject().contentWindow;
                  if (contentWindow.setClientApiContext) {
                    clearInterval(token);
                    contentWindow.setClientApiContext(Xrm, formContext);
                    if (successCallback) {
                      successCallback(contentWindow);
                    }
                  }
                }, 500);

                // timeout after 2 mins of trying
                setTimeout(clearInterval, 2 * 60 * 60 * 1000, token);
              }
            }
            catch (err) {
              if (failedCallback) {
                failedCallback(err);
              }
            }
          }
        }
      }
    },
  };
}